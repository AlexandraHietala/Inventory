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
    public interface IUpdateSeriesWorkflowV1
    {
        Task UpdateSeries(Series series);
    }

    public class UpdateSeriesWorkflowV1 : IUpdateSeriesWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateSeriesOperationsV1 _updateSeriesOperations;
        private readonly ISeriesDataValidatorV1 _dataValidator;
        private readonly ISeriesWorkflowValidatorV1 _workflowValidator;

        public UpdateSeriesWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateSeriesWorkflowV1>();
            _configuration = configuration;
            _updateSeriesOperations = new UpdateSeriesOperationsV1(loggerFactory, configuration);
            _dataValidator = new SeriesDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new SeriesWorkflowValidatorV1(loggerFactory, configuration, _dataValidator);
        }

        public async Task UpdateSeries(Series series)
        {
            _logger.LogDebug("UpdateSeries request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateUpdateSeries(series);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                SeriesDto seriesDto = SeriesConverter.ConvertSeriesToSeriesDto(series);
                await _updateSeriesOperations.UpdateSeries(seriesDto);

                // Respond
                _logger.LogInformation("UpdateSeries success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400300009] UpdateSeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[400300010] UpdateSeries Exception: {e}.");
                throw;
            }
        }
    }
}