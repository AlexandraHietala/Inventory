using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Workflows.Workflows.V1
{
    public interface IUpdateItemCommentWorkflowV1
    {
        Task UpdateItemComment(ItemComment comment);
    }

    public class UpdateItemCommentWorkflowV1 : IUpdateItemCommentWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateItemCommentOperationsV1 _updateItemCommentOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IItemCommentWorkflowValidatorV1 _workflowValidator;

        public UpdateItemCommentWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemCommentWorkflowV1>();
            _configuration = configuration;
            _updateItemCommentOperations = new UpdateItemCommentOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new ItemCommentWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task UpdateItemComment(ItemComment comment)
        {
            _logger.LogDebug("UpdateItemComment request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateUpdateItemComment(comment);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemCommentDto commentDto = ItemCommentConverter.ConvertItemCommentToItemCommentDto(comment);
                await _updateItemCommentOperations.UpdateItemComment(commentDto);

                // Respond
                _logger.LogInformation("UpdateItemComment success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300035] UpdateItemComment ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300036] UpdateItemComment Exception: {e}.");
                throw;
            }
        }
    }
}