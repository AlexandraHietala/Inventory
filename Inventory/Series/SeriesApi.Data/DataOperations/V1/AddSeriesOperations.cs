using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using SeriesApi.Models.DTOs.V1;

namespace SeriesApi.Data.DataOperations.V1
{
    public interface IAddSeriesOperationsV1
    {
        Task<int> AddSeries(SeriesDto series);
    }

    public class AddSeriesOperationsV1 : IAddSeriesOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public AddSeriesOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddSeriesOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task<int> AddSeries(SeriesDto series)
        {
            try
            {
                _logger.LogDebug("AddSeries request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                int id = await connection.QueryFirstAsync<int>("[app].[spAddSeries]", new { series_name = series.SERIES_NAME, description = series.SERIES_DESCRIPTION, lastmodifiedby = series.SERIES_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("AddSeries success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500019] AddSeries Error while inserting series: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500020] AddSeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500021] AddSeries Exception: {e}.");
                throw;
            }
        }
    }
}
