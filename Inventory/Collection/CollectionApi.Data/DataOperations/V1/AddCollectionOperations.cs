using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using CollectionApi.Models.DTOs.V1;

namespace CollectionApi.Data.DataOperations.V1
{
    public interface IAddCollectionOperationsV1
    {
        Task<int> AddCollection(CollectionDto collection);
    }

    public class AddCollectionOperationsV1 : IAddCollectionOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public AddCollectionOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddCollectionOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenCollection")!;
        }

        public async Task<int> AddCollection(CollectionDto collection)
        {
            try
            {
                _logger.LogDebug("AddCollection request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                int id = await connection.QueryFirstAsync<int>("[app].[spAddCollection]", new { collection_name = collection.COLLECTION_NAME, description = collection.COLLECTION_DESCRIPTION, lastmodifiedby = collection.COLLECTION_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("AddCollection success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[500500001] AddCollection Error while inserting collection: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[500500002] AddCollection InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[500500003] AddCollection Exception: {e}.");
                throw;
            }
        }
    }
}
