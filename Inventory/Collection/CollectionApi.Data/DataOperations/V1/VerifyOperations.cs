using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using CollectionApi.Models.DTOs.V1;

namespace CollectionApi.Data.DataOperations.V1
{
    public interface IVerifyOperationsV1
    {
        Task<bool> VerifyCollection(int id);
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
            _connString = _configuration.GetConnectionString("StarryEdenCollection")!;
        }

        public async Task<bool> VerifyCollection(int id)
        {
            try
            {
                _logger.LogDebug("VerifyCollection request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                CollectionDto collection = await connection.QueryFirstAsync<CollectionDto>("[app].[spGetCollection]", new { id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("VerifyCollection success response.");
                if (collection != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyCollection InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyCollection Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
