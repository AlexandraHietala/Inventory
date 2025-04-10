using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeriesApi.Workflows.Validators.V1;
using SeriesApi.Models.Classes.V1;
using SeriesApi.Models.Converters.V1;
using SeriesApi.Models.DTOs.V1;
using SeriesApi.Data.Validators.V1;
using SeriesApi.Data.DataOperations.V1;

namespace SeriesApi.Workflows.Workflows.V1
{
    public interface IGetSeriesWorkflowV1
    {
        Task<Series> GetASeries(int id);
        Task<List<Series>> GetSeries();
    }

    public class GetSeriesWorkflowV1 : IGetSeriesWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetSeriesOperationsV1 _getSeriesOperations;
        private readonly ISeriesDataValidatorV1 _dataValidator;
        private readonly ISeriesWorkflowValidatorV1 _workflowValidator;

        public GetSeriesWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetSeriesWorkflowV1>();
            _configuration = configuration;
            _getSeriesOperations = new GetSeriesOperationsV1(loggerFactory, configuration);
            _dataValidator = new SeriesDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new SeriesWorkflowValidatorV1(loggerFactory, configuration, _dataValidator);
        }

        public async Task<Series> GetASeries(int id)
        {
            _logger.LogDebug("GetASeries request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateSeriesId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                SeriesDto seriesDto = await _getSeriesOperations.GetASeries(id);
                Series series = SeriesConverter.ConvertSeriesDtoToSeries(seriesDto);

                // Respond
                _logger.LogInformation("GetASeries success response.");
                return series;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400300003] GetASeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[400300004] GetSeries Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Series>> GetSeries()
        {
            _logger.LogDebug("GetSeries request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<SeriesDto> seriesDtos = await _getSeriesOperations.GetSeries(null);
                List<Series> series = SeriesConverter.ConvertListSeriesDtoToListSeries(seriesDtos);

                // Respond
                _logger.LogInformation("GetASeries success response.");
                return series;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400300005] GetASeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[400300006] GetASeries Exception: {e}.");
                throw;
            }
        }
    }
}