using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;
using Azure.Identity;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IGetSeriesOperationsV1
    {
        Task<SeriesDto> GetASeries(int id);
        Task<List<SeriesDto>> GetSeries(string? search);
    }

    public class GetSeriesOperationsV1 : IGetSeriesOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public GetSeriesOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetSeriesOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task<SeriesDto> GetASeries(int id)
        {
            try
            {
                _logger.LogDebug("GetASeries request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                SeriesDto series = await connection.QueryFirstAsync<SeriesDto>("[app].[spGetASeries]", new { id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("GetASeries success response.");
                return series;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This series doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetASeries InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500037] Series does not exist.");
                }
                else
                {
                    _logger.LogError($"[200500038] GetASeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500039] GetASeries Exception: {e}.");
                throw;
            }
        }
        public async Task<List<SeriesDto>> GetSeries(string? search)
        {
            try
            {
                _logger.LogDebug("GetSeries request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                IEnumerable<SeriesDto> seriess = await connection.QueryAsync<SeriesDto>("[app].[spGetSeries]", new { search = search }, commandType: CommandType.StoredProcedure);
                _logger.LogInformation("GetSeries success response.");
                return seriess.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This series doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetSeries InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500040] Seriess do not exist.");
                }
                else
                {
                    _logger.LogError($"[200500041] GetSeries InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500042] GetSeries Exception: {e}.");
                throw;
            }
        }
    }
}
