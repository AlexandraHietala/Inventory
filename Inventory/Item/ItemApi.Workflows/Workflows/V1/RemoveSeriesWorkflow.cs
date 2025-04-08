using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Workflows.Validators.V1;

namespace ItemApi.Workflows.Workflows.V1
{
    public interface IRemoveSeriesWorkflowV1
    {
        Task RemoveSeries(int id, string lastmodifiedby);
    }

    public class RemoveSeriesWorkflowV1 : IRemoveSeriesWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveSeriesOperationsV1 _removeSeriesOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly ISeriesWorkflowValidatorV1 _workflowValidator;

        public RemoveSeriesWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveSeriesWorkflowV1>();
            _configuration = configuration;
            _removeSeriesOperations = new RemoveSeriesOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new SeriesWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task RemoveSeries(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveSeries request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateSeriesId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeSeriesOperations.RemoveSeries(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveSeries success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300031] RemoveSeries ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300032] RemoveSeries Exception: {e}.");
                throw;
            }
        }
    }
}