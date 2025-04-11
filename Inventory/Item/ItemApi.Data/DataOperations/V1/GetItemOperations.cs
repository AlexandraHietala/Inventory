using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;
using Azure.Identity;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IGetItemOperationsV1
    {
        Task<ItemDto> GetItem(int id);
        Task<List<ItemDto>> GetItems(string? search);
    }

    public class GetItemOperationsV1 : IGetItemOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public GetItemOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task<ItemDto> GetItem(int id)
        {
            try
            {
                _logger.LogDebug("GetItem request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                ItemDto item = await connection.QueryFirstAsync<ItemDto>("[app].[spGetItem]", new { id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("GetItem success response.");
                return item;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This item doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetItem InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500013] Item does not exist.");
                }
                else
                {
                    _logger.LogError($"[200500014] GetItem InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500015] GetItem Exception: {e}.");
                throw;
            }
        }
        public async Task<List<ItemDto>> GetItems(string? search)
        {
            try
            {
                _logger.LogDebug("GetItems request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                IEnumerable<ItemDto> items = await connection.QueryAsync<ItemDto>("[app].[spGetItems]", new { search = search }, commandType: CommandType.StoredProcedure);
                _logger.LogInformation("GetItems success response.");
                return items.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This item doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetItems InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500016] Items do not exist.");
                }
                else
                {
                    _logger.LogError($"[200500017] GetItems InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500018] GetItems Exception: {e}.");
                throw;
            }
        } 
    }
}
