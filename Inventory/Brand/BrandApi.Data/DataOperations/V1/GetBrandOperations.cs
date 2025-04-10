using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using BrandApi.Models.DTOs.V1;
using Azure.Identity;

namespace BrandApi.Data.DataOperations.V1
{
    public interface IGetBrandOperationsV1
    {
        Task<BrandDto> GetBrand(int id);
        Task<List<BrandDto>> GetBrands(string? search);
    }

    public class GetBrandOperationsV1 : IGetBrandOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public GetBrandOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetBrandOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenBrand")!;
        }

        public async Task<BrandDto> GetBrand(int id)
        {
            try
            {
                _logger.LogDebug("GetBrand request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                BrandDto brand = await connection.QueryFirstAsync<BrandDto>("[app].[spGetBrand]", new { id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("GetBrand success response.");
                return brand;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This brand doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetBrand InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500004] Brand does not exist.");
                }
                else
                {
                    _logger.LogError($"[300500005] GetBrand InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[300500006] GetBrand Exception: {e}.");
                throw;
            }
        }
        public async Task<List<BrandDto>> GetBrands(string? search)
        {
            try
            {
                _logger.LogDebug("GetBrands request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                IEnumerable<BrandDto> brands = await connection.QueryAsync<BrandDto>("[app].[spGetBrands]", new { search = search }, commandType: CommandType.StoredProcedure);
                _logger.LogInformation("GetBrands success response.");
                return brands.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This brand doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetBrands InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500007] Brands do not exist.");
                }
                else
                {
                    _logger.LogError($"[300500008] GetBrands InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[300500009] GetBrands Exception: {e}.");
                throw;
            }
        }
    }
}
