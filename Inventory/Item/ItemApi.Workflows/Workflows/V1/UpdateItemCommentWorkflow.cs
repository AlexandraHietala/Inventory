using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;
using ItemApi.Data.Validators.V1;

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
        private readonly IItemDataValidatorV1 _itemDataValidator;
        private readonly IItemCommentDataValidatorV1 _itemCommentDataValidator;
        private readonly IItemCommentWorkflowValidatorV1 _workflowValidator;

        public UpdateItemCommentWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemCommentWorkflowV1>();
            _configuration = configuration;
            _updateItemCommentOperations = new UpdateItemCommentOperationsV1(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidatorV1(loggerFactory, configuration);
            _itemCommentDataValidator = new ItemCommentDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new ItemCommentWorkflowValidatorV1(loggerFactory, configuration, _itemDataValidator, _itemCommentDataValidator);
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
                _logger.LogError($"[200300019] UpdateItemComment ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300020] UpdateItemComment Exception: {e}.");
                throw;
            }
        }
    }
}