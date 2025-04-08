using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Workflows.Workflows.V1
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
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly ISeriesWorkflowValidatorV1 _workflowValidator;

        public GetSeriesWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetSeriesWorkflowV1>();
            _configuration = configuration;
            _getSeriesOperations = new GetSeriesOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new SeriesWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
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
                _logger.LogError($"[200300021] GetASeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300022] GetSeries Exception: {e}.");
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
                _logger.LogError($"[200300023] GetASeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300024] GetASeries Exception: {e}.");
                throw;
            }
        }
    }
}