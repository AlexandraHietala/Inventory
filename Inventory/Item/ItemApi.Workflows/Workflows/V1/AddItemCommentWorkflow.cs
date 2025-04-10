using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.Validators.V1;

namespace ItemApi.Workflows.Workflows.V1
{
    public interface IAddItemCommentWorkflowV1
    {
        Task<int> AddItemComment(ItemComment comment);
    }

    public class AddItemCommentWorkflowV1 : IAddItemCommentWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddItemCommentOperationsV1 _addItemCommentOperations;
        private readonly IItemDataValidatorV1 _itemDataValidator;
        private readonly IItemCommentDataValidatorV1 _itemCommentDataValidator;
        private readonly IItemCommentWorkflowValidatorV1 _workflowValidator;


        public AddItemCommentWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemCommentWorkflowV1>();
            _configuration = configuration;
            _addItemCommentOperations = new AddItemCommentOperationsV1(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidatorV1(loggerFactory, configuration);
            _itemCommentDataValidator = new ItemCommentDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new ItemCommentWorkflowValidatorV1(loggerFactory, configuration, _itemDataValidator, _itemCommentDataValidator);
        }

        public async Task<int> AddItemComment(ItemComment comment)
        {
            _logger.LogDebug("AddItemComment request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateAddItemComment(comment);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemCommentDto commentDto = ItemCommentConverter.ConvertItemCommentToItemCommentDto(comment);
                int newCommentId = await _addItemCommentOperations.AddItemComment(commentDto);

                // Respond
                _logger.LogInformation("AddItemComment success response.");
                return newCommentId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300001] AddItemComment ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300002] AddItemComment Exception: {e}.");
                throw;
            }
        }
    }
}