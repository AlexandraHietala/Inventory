using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CollectionApi.Data.DataOperations.V1;
using CollectionApi.Workflows.Validators.V1;

namespace CollectionApi.Workflows.Workflows.V1
{
    public interface IRemoveCollectionWorkflowV1
    {
        Task RemoveCollection(int id, string lastmodifiedby);
    }

    public class RemoveCollectionWorkflowV1 : IRemoveCollectionWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveCollectionOperationsV1 _removeCollectionOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly ICollectionWorkflowValidatorV1 _workflowValidator;

        public RemoveCollectionWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveCollectionWorkflowV1>();
            _configuration = configuration;
            _removeCollectionOperations = new RemoveCollectionOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new CollectionWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task RemoveCollection(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveCollection request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateCollectionId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeCollectionOperations.RemoveCollection(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveCollection success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300300007] RemoveCollection ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[300300008] RemoveCollection Exception: {e}.");
                throw;
            }
        }
    }
}