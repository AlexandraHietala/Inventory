using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using BrandApi.Models.DTOs.V1;

namespace BrandApi.Data.DataOperations.V1
{
    public interface IVerifyOperationsV1
    {
        Task<bool> VerifyBrand(int id);
    }

    public class VerifyOperationsV1 : IVerifyOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public VerifyOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<VerifyOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenBrand")!;
        }

        public async Task<bool> VerifyBrand(int id)
        {
            try
            {
                _logger.LogDebug("VerifyBrand request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                BrandDto brand = await connection.QueryFirstAsync<BrandDto>("[app].[spGetBrand]", new { id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("VerifyBrand success response.");
                if (brand != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyBrand InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyBrand Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }

    }
}
