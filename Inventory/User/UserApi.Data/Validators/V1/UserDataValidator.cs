using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using UserApi.Models.DTOs.V1;

namespace UserApi.Data.Validators.V1
{
    public interface IUserDataValidatorV1
    {
        Task<bool> VerifyUser(int id);
    }

    public class UserDataValidatorV1 : IUserDataValidatorV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public UserDataValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UserDataValidatorV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenUser")!;
        }

        public async Task<bool> VerifyUser(int id)
        {
            try
            {
                _logger.LogDebug("VerifyUser request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                UserDto user = await connection.QueryFirstAsync<UserDto>("[app].[spGetUser]", new { id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("VerifyUser success response.");
                if (user != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyUser InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyUser Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
