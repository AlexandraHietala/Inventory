using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeriesApi.Models.Classes.V1;
using SeriesApi.Models.Converters.V1;
using SeriesApi.Models.DTOs.V1;
using SeriesApi.Workflows.Validators.V1;
using SeriesApi.Data.Validators.V1;
using SeriesApi.Data.DataOperations.V1;

namespace SeriesApi.Workflows.Workflows.V1
{
    public interface IAddSeriesWorkflowV1
    {
        Task<int> AddSeries(Series series);
    }

    public class AddSeriesWorkflowV1 : IAddSeriesWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddSeriesOperationsV1 _addSeriesOperations;
        private readonly ISeriesDataValidatorV1 _dataValidator;
        private readonly ISeriesWorkflowValidatorV1 _workflowValidator;


        public AddSeriesWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddSeriesWorkflowV1>();
            _configuration = configuration;
            _addSeriesOperations = new AddSeriesOperationsV1(loggerFactory, configuration);
            _dataValidator = new SeriesDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new SeriesWorkflowValidatorV1(loggerFactory, configuration, _dataValidator);
        }

        public async Task<int> AddSeries(Series series)
        {
            _logger.LogDebug("AddSeries request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateAddSeries(series);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                SeriesDto seriesDto = SeriesConverter.ConvertSeriesToSeriesDto(series);
                int newSeriesId = await _addSeriesOperations.AddSeries(seriesDto);

                // Respond
                _logger.LogInformation("AddSeries success response.");
                return newSeriesId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400300001] AddSeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[400300002] AddSeries Exception: {e}.");
                throw;
            }
        }
    }
}