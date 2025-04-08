using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Workflows.Workflows.V1
{
    public interface IGetItemCommentWorkflowV1
    {
        Task<ItemComment> GetItemComment(int id);
        Task<List<ItemComment>> GetItemComments(int itemId);
    }

    public class GetItemCommentWorkflowV1 : IGetItemCommentWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetItemCommentOperationsV1 _getItemCommentOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IItemCommentWorkflowValidatorV1 _workflowValidator;

        public GetItemCommentWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemCommentWorkflowV1>();
            _configuration = configuration;
            _getItemCommentOperations = new GetItemCommentOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new ItemCommentWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task<ItemComment> GetItemComment(int id)
        {
            _logger.LogDebug("GetItemComment request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateItemCommentId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemCommentDto commentDto = await _getItemCommentOperations.GetItemComment(id);
                ItemComment comment = ItemCommentConverter.ConvertItemCommentDtoToItemComment(commentDto);

                // Respond
                _logger.LogInformation("GetItemComment success response.");
                return comment;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300013] GetItemComment ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300014] GetItemComment Exception: {e}.");
                throw;
            }
        }

        public async Task<List<ItemComment>> GetItemComments(int itemId)
        {
            _logger.LogDebug("GetItemComments request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateItemId(itemId);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                List<ItemCommentDto> commentDtos = await _getItemCommentOperations.GetItemComments(itemId);
                List<ItemComment> comments = ItemCommentConverter.ConvertListItemCommentDtoToListItemComment(commentDtos);

                // Respond
                _logger.LogInformation("GetItemComments success response.");
                return comments;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300015] GetItemComments ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300016] GetItemComments Exception: {e}.");
                throw;
            }
        }
    }
}