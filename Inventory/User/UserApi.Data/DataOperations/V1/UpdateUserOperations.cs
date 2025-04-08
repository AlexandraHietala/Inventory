using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using UserApi.Models.DTOs.V1;

namespace UserApi.Data.DataOperations.V1
{
    public interface IUpdateUserOperationsV1
    {
        Task UpdateUser(UserDto user);
    }

    public class UpdateUserOperationsV1 : IUpdateUserOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public UpdateUserOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateUserOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenUser")!;
        }

        public async Task UpdateUser(UserDto user)
        {
            try
            {
                _logger.LogDebug("UpdateUser request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spUpdateUser]", new { id = user.ID, name = user.NAME, salt = user.PASS_SALT, hash = user.PASS_HASH, role = user.ROLE_ID, lastmodifiedby = user.LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("UpdateUser success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[100500022] UpdateUser Error while updating user: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[100500023] UpdateUser InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500024] UpdateUser Exception: {e}.");
                throw;
            }
        }
    }
}
