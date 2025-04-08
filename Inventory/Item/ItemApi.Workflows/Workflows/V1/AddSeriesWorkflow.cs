using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;
using ItemApi.Workflows.Validators.V1;

namespace ItemApi.Workflows.Workflows.V1
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
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly ISeriesWorkflowValidatorV1 _workflowValidator;


        public AddSeriesWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddSeriesWorkflowV1>();
            _configuration = configuration;
            _addSeriesOperations = new AddSeriesOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new SeriesWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
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
                _logger.LogError($"[200300007] AddSeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300008] AddSeries Exception: {e}.");
                throw;
            }
        }
    }
}