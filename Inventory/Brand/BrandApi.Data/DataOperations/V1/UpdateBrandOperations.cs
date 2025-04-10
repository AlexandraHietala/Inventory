using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using BrandApi.Models.DTOs.V1;

namespace BrandApi.Data.DataOperations.V1
{
    public interface IUpdateBrandOperationsV1
    {
        Task UpdateBrand(BrandDto brand);
    }

    public class UpdateBrandOperationsV1 : IUpdateBrandOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public UpdateBrandOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateBrandOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenBrand")!;
        }

        public async Task UpdateBrand(BrandDto brand)
        {
            try
            {
                _logger.LogDebug("UpdateBrand request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spUpdateBrand]", new { id = brand.BRAND_ID, brand_name = brand.BRAND_NAME, description = brand.BRAND_DESCRIPTION, lastmodifiedby = brand.BRAND_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("UpdateBrand success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500052] UpdateBrand Error while updating brand: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500053] UpdateBrand InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500054] UpdateBrand Exception: {e}.");
                throw;
            }
        }
    }
}
