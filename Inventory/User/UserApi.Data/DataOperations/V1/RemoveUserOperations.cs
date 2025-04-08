using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using UserApi.Models.DTOs.V1;

namespace UserApi.Data.DataOperations.V1
{
    public interface IRemoveUserOperationsV1
    {
        Task RemoveUser(int id, string lastmodifiedby);
    }

    public class RemoveUserOperationsV1 : IRemoveUserOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public RemoveUserOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveUserOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenUser")!;
        }

        public async Task RemoveUser(int id, string lastmodifiedby)
        {
            try
            {
                _logger.LogDebug("RemoveUser request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spRemoveUser]", new { id, lastmodifiedby }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("RemoveUser success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[100500019] Error while removing user: {ioe}.");
                    throw;
                }
                else
                {
                    _logger.LogError($"[100500020] RemoveUser InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500021] RemoveUser Exception: {e}.");
                throw;
            }
        }

    }
}
