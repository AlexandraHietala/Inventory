using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IRemoveBrandOperationsV1
    {
        Task RemoveBrand(int id, string lastmodifiedby);
    }

    public class RemoveBrandOperationsV1 : IRemoveBrandOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public RemoveBrandOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveBrandOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task RemoveBrand(int id, string lastmodifiedby)
        {
            try
            {
                _logger.LogDebug("RemoveBrand request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spRemoveBrand]", new { id = id, lastmodifiedby = lastmodifiedby }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("RemoveBrand success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500043] Error while removing brand: {ioe}.");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500044] RemoveBrand InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500045] RemoveBrand Exception: {e}.");
                throw;
            }
        }

    }
}
