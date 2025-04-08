using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Workflows.Validators.V1;

namespace ItemApi.Workflows.Workflows.V1
{
    public interface IRemoveItemWorkflowV1
    {
        Task RemoveItem(int id, string lastmodifiedby);
    }

    public class RemoveItemWorkflowV1 : IRemoveItemWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveItemOperationsV1 _removeItemOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IItemWorkflowValidatorV1 _workflowValidator;

        public RemoveItemWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemWorkflowV1>();
            _configuration = configuration;
            _removeItemOperations = new RemoveItemOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new ItemWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task RemoveItem(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveItem request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateItemId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeItemOperations.RemoveItem(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveItem success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300029] RemoveItem ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300030] RemoveItem Exception: {e}.");
                throw;
            }
        }
    }
}