using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.Validators.V1;

namespace ItemApi.Workflows.Workflows.V1
{
    public interface IRemoveItemCommentWorkflowV1
    {
        Task RemoveItemComment(int id, string lastmodifiedby);
    }

    public class RemoveItemCommentWorkflowV1 : IRemoveItemCommentWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveItemCommentOperationsV1 _removeItemCommentOperations;
        private readonly IItemDataValidatorV1 _itemDataValidator;
        private readonly IItemCommentDataValidatorV1 _itemCommentDataValidator;
        private readonly IItemCommentWorkflowValidatorV1 _workflowValidator;

        public RemoveItemCommentWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemCommentWorkflowV1>();
            _configuration = configuration;
            _removeItemCommentOperations = new RemoveItemCommentOperationsV1(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidatorV1(loggerFactory, configuration);
            _itemCommentDataValidator = new ItemCommentDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new ItemCommentWorkflowValidatorV1(loggerFactory, configuration, _itemDataValidator, _itemCommentDataValidator);
        }

        
        public async Task RemoveItemComment(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveItemComment request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateItemCommentId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeItemCommentOperations.RemoveItemComment(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveItemComment success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300015] RemoveItemComment ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300016] RemoveItemComment Exception: {e}.");
                throw;
            }
        }
    }
}