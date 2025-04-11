using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IUpdateItemOperationsV1
    {
        Task UpdateItem(ItemDto item);
    }

    public class UpdateItemOperationsV1 : IUpdateItemOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public UpdateItemOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task UpdateItem(ItemDto item)
        {
            try
            {
                _logger.LogDebug("UpdateItem request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spUpdateItem]", new { id = item.ID, status = item.STATUS, type = item.TYPE, brand_id = item.BRAND, series_id = item.SERIES, name = item.NAME, description = item.DESCRIPTION, format = item.FORMAT, size = item.SIZE, year = item.YEAR, photo = item.PHOTO, lastmodifiedby = item.LAST_MODIFIED_BY  }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("UpdateItem success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500031] UpdateItem Error while updating item: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500032] UpdateItem InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500033] UpdateItem Exception: {e}.");
                throw;
            }
        }
    }
}
