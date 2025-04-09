using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using CollectionApi.Models.DTOs.V1;

namespace CollectionApi.Data.DataOperations.V1
{
    public interface IUpdateCollectionOperationsV1
    {
        Task UpdateCollection(CollectionDto collection);
    }

    public class UpdateCollectionOperationsV1 : IUpdateCollectionOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public UpdateCollectionOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateCollectionOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenCollection")!;
        }

        public async Task UpdateCollection(CollectionDto collection)
        {
            try
            {
                _logger.LogDebug("UpdateCollection request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spUpdateCollection]", new { id = collection.COLLECTION_ID, collection_name = collection.COLLECTION_NAME, description = collection.COLLECTION_DESCRIPTION, lastmodifiedby = collection.COLLECTION_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("UpdateCollection success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[300500013] UpdateCollection Error while updating collection: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[300500014] UpdateCollection InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[300500015] UpdateCollection Exception: {e}.");
                throw;
            }
        }
    }
}
