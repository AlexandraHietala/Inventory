using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using CollectionApi.Models.DTOs.V1;
using Azure.Identity;

namespace CollectionApi.Data.DataOperations.V1
{
    public interface IGetCollectionOperationsV1
    {
        Task<CollectionDto> GetCollection(int id);
        Task<List<CollectionDto>> GetCollections(string? search);
    }

    public class GetCollectionOperationsV1 : IGetCollectionOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public GetCollectionOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetCollectionOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenCollection")!;
        }

        public async Task<CollectionDto> GetCollection(int id)
        {
            try
            {
                _logger.LogDebug("GetCollection request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                CollectionDto collection = await connection.QueryFirstAsync<CollectionDto>("[app].[spGetCollection]", new { id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("GetCollection success response.");
                return collection;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This collection doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetCollection InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[300500004] Collection does not exist.");
                }
                else
                {
                    _logger.LogError($"[300500005] GetCollection InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[300500006] GetCollection Exception: {e}.");
                throw;
            }
        }
        public async Task<List<CollectionDto>> GetCollections(string? search)
        {
            try
            {
                _logger.LogDebug("GetCollections request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                IEnumerable<CollectionDto> collections = await connection.QueryAsync<CollectionDto>("[app].[spGetCollections]", new { search = search }, commandType: CommandType.StoredProcedure);
                _logger.LogInformation("GetCollections success response.");
                return collections.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This collection doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetCollections InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[300500007] Collections do not exist.");
                }
                else
                {
                    _logger.LogError($"[300500008] GetCollections InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[300500009] GetCollections Exception: {e}.");
                throw;
            }
        }
    }
}
