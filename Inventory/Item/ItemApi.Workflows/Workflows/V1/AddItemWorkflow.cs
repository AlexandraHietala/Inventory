using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;
using ItemApi.Workflows.Validators.V1;

namespace ItemApi.Workflows.Workflows.V1
{
    public interface IAddItemWorkflowV1
    {
        Task<int> AddItem(Item item);
    }

    public class AddItemWorkflowV1 : IAddItemWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddItemOperationsV1 _addItemOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IItemWorkflowValidatorV1 _workflowValidator;


        public AddItemWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemWorkflowV1>();
            _configuration = configuration;
            _addItemOperations = new AddItemOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new ItemWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }
        
        public async Task<int> AddItem(Item item)
        {
            _logger.LogDebug("AddItem request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateAddItem(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemDto itemDto = ItemConverter.ConvertItemToItemDto(item);
                int newItemId = await _addItemOperations.AddItem(itemDto);

                // Respond
                _logger.LogInformation("AddItem success response.");
                return newItemId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300005] AddItem ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300006] AddItem Exception: {e}.");
                throw;
            }
        }
    }
}