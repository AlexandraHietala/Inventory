using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IRemoveSeriesOperationsV1
    {
        Task RemoveSeries(int id, string lastmodifiedby);
    }

    public class RemoveSeriesOperationsV1 : IRemoveSeriesOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public RemoveSeriesOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveSeriesOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task RemoveSeries(int id, string lastmodifiedby)
        {
            try
            {
                _logger.LogDebug("RemoveSeries request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spRemoveSeries]", new { id = id, lastmodifiedby = lastmodifiedby }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("RemoveSeries success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500049] Error while removing series: {ioe}.");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500050] RemoveSeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500051] RemoveSeries Exception: {e}.");
                throw;
            }
        }

    }
}
