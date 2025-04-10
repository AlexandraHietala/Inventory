using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using UserApi.Models.DTOs.V1;

namespace UserApi.Data.DataOperations.V1
{
    public interface IRoleDataValidatorV1
    {
        Task<bool> VerifyRole(int id);
    }

    public class RoleDataValidatorV1 : IRoleDataValidatorV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public RoleDataValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RoleDataValidatorV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenUser")!;
        }

        public async Task<bool> VerifyRole(int id)
        {
            try
            {
                _logger.LogDebug("VerifyRole request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                RoleDto role = await connection.QueryFirstAsync<RoleDto>("[app].[spGetRole]", new { id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("VerifyRole success response.");
                if (role != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyRole InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyRole Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
