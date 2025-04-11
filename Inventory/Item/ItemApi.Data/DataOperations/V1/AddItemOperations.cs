using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IAddItemOperationsV1
    {
        Task<int> AddItem(ItemDto item);
    }

    public class AddItemOperationsV1 : IAddItemOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public AddItemOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task<int> AddItem(ItemDto item)
        {
            try
            {
                _logger.LogDebug("AddItem request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                int id = await connection.QueryFirstAsync<int>("[app].[spAddItem]", new { status = item.STATUS, type = item.TYPE, brand = item.BRAND, series  = item.SERIES, name = item.NAME, description = item.DESCRIPTION, format = item.FORMAT, size = item.SIZE, year = item.YEAR, photo = item.PHOTO, lastmodifiedby = item.LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("AddItem success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500004] AddItem Error while inserting item: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500005] AddItem InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500006] AddItem Exception: {e}.");
                throw;
            }
        }
    }
}
