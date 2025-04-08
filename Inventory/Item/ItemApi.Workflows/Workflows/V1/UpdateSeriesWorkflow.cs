using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Workflows.Workflows.V1
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
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly ISeriesWorkflowValidatorV1 _workflowValidator;

        public UpdateSeriesWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateSeriesWorkflowV1>();
            _configuration = configuration;
            _updateSeriesOperations = new UpdateSeriesOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new SeriesWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
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
                _logger.LogError($"[200300039] UpdateSeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300040] UpdateSeries Exception: {e}.");
                throw;
            }
        }
    }
}