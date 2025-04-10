using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using CollectionApi.Models.DTOs.V1;

namespace CollectionApi.Data.DataOperations.V1
{
    public interface IRemoveCollectionOperationsV1
    {
        Task RemoveCollection(int id, string lastmodifiedby);
    }

    public class RemoveCollectionOperationsV1 : IRemoveCollectionOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public RemoveCollectionOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveCollectionOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenCollection")!;
        }

        public async Task RemoveCollection(int id, string lastmodifiedby)
        {
            try
            {
                _logger.LogDebug("RemoveCollection request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spRemoveCollection]", new { id = id, lastmodifiedby = lastmodifiedby }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("RemoveCollection success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[500500010] Error while removing collection: {ioe}.");
                    throw;
                }
                else
                {
                    _logger.LogError($"[500500011] RemoveCollection InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[500500012] RemoveCollection Exception: {e}.");
                throw;
            }
        }

    }
}
