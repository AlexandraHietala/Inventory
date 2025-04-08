using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;
using ItemApi.Workflows.Validators.V1;

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
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IItemCommentWorkflowValidatorV1 _workflowValidator;


        public AddItemCommentWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemCommentWorkflowV1>();
            _configuration = configuration;
            _addItemCommentOperations = new AddItemCommentOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new ItemCommentWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
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
                _logger.LogError($"[200300003] AddItemComment ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300004] AddItemComment Exception: {e}.");
                throw;
            }
        }
    }
}