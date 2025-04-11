using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.Validators.V1
{
    public interface IItemDataValidatorV1
    {
        Task<bool> VerifyItem(int id);
    }

    public class ItemDataValidatorV1 : IItemDataValidatorV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public ItemDataValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<ItemDataValidatorV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task<bool> VerifyItem(int id)
        {
            try
            {
                _logger.LogDebug("VerifyItem request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                ItemDto item = await connection.QueryFirstAsync<ItemDto>("[app].[spGetItem]", new { id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("VerifyItem success response.");
                if (item != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyItem InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyItem Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
