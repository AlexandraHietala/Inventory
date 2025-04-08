using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IAddBrandOperationsV1
    {
        Task<int> AddBrand(BrandDto brand);
    }

    public class AddBrandOperationsV1 : IAddBrandOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public AddBrandOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddBrandOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task<int> AddBrand(BrandDto brand)
        {
            try
            {
                _logger.LogDebug("AddBrand request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                int id = await connection.QueryFirstAsync<int>("[app].[spAddBrand]", new { brand_name = brand.BRAND_NAME, description = brand.BRAND_DESCRIPTION, lastmodifiedby = brand.BRAND_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("AddBrand success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500013] AddBrand Error while inserting brand: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500014] AddBrand InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500015] AddBrand Exception: {e}.");
                throw;
            }
        }
    }
}
