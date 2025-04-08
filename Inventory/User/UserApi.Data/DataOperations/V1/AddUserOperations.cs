using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using UserApi.Models.DTOs.V1;

namespace UserApi.Data.DataOperations.V1
{
    public interface IAddUserOperationsV1
    {
        Task<int> AddUser(UserDto user);
    }

    public class AddUserOperationsV1 : IAddUserOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public AddUserOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddUserOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenUser")!;
        }

        public async Task<int> AddUser(UserDto user)
        {
            try
            {
                _logger.LogDebug("AddUser request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                int id = await connection.QueryFirstAsync<int>("[app].[spAddUser]", new { name = user.NAME, salt = user.PASS_SALT, hash = user.PASS_HASH, role = user.ROLE_ID, lastmodifiedby = user.LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("AddUser success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[100500001] AddUser Error while inserting user: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[100500002] AddUser InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500003] AddUser Exception: {e}.");
                throw;
            }
        }
    }
}
