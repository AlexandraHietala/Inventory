using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using UserApi.Models.DTOs.V1;
using UserApi.Models.Classes.V1;

namespace UserApi.Data.DataOperations.V1
{
    public interface IGetUserOperationsV1
    {
        Task<UserDto> GetUser(int id);
        Task<List<UserDto>> GetUsers();
    }

    public class GetUserOperationsV1 : IGetUserOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public GetUserOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetUserOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenUser")!;
        }

        public async Task<UserDto> GetUser(int id)
        {
            try
            {
                _logger.LogDebug("GetUser request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                UserDto user = await connection.QueryFirstAsync<UserDto>("[app].[spGetUser]", new { id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("GetUser success response.");
                return user;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This user doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetUser InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[100500013] User does not exist.");
                }
                else
                {
                    _logger.LogError($"[100500014] GetUser InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500015] GetUser Exception: {e}.");
                throw;
            }
        }
        public async Task<List<UserDto>> GetUsers()
        {
            try
            {
                _logger.LogDebug("GetUsers request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                IEnumerable<UserDto> users = await connection.QueryAsync<UserDto>("[app].[spGetUsers]", new { }, commandType: CommandType.StoredProcedure);
                _logger.LogInformation("GetUsers success response.");
                return users.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This user doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetUsers InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[100500016] Users do not exist.");
                }
                else
                {
                    _logger.LogError($"[100500017] GetUsers InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500018] GetUsers Exception: {e}.");
                throw;
            }
        }
    }
}
