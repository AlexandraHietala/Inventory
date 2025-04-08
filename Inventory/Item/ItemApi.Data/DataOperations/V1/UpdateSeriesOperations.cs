using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IUpdateSeriesOperationsV1
    {
        Task UpdateSeries(SeriesDto series);
    }

    public class UpdateSeriesOperationsV1 : IUpdateSeriesOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public UpdateSeriesOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateSeriesOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task UpdateSeries(SeriesDto series)
        {
            try
            {
                _logger.LogDebug("UpdateSeries request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spUpdateSeries]", new { id = series.SERIES_ID, series_name = series.SERIES_NAME, description = series.SERIES_DESCRIPTION, lastmodifiedby = series.SERIES_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("UpdateSeries success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500058] UpdateSeries Error while updating series: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500059] UpdateSeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500060] UpdateSeries Exception: {e}.");
                throw;
            }
        }
    }
}
