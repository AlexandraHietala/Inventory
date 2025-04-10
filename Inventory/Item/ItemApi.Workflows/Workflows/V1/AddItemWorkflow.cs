using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.Validators.V1;
using SeriesApi.Data.Validators.V1;
using BrandApi.Data.Validators.V1;
using CollectionApi.Data.Validators.V1;

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
        private readonly IItemDataValidatorV1 _itemDataValidator;
        private readonly ISeriesDataValidatorV1 _seriesDataValidator;
        private readonly IBrandDataValidatorV1 _brandDataValidator;
        private readonly ICollectionDataValidatorV1 _collectionDataValidator;
        private readonly IItemWorkflowValidatorV1 _workflowValidator;


        public AddItemWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemWorkflowV1>();
            _configuration = configuration;
            _addItemOperations = new AddItemOperationsV1(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidatorV1(loggerFactory, configuration);
            _seriesDataValidator = new SeriesDataValidatorV1(loggerFactory, configuration);
            _brandDataValidator = new BrandDataValidatorV1(loggerFactory, configuration);
            _collectionDataValidator = new CollectionDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new ItemWorkflowValidatorV1(loggerFactory, configuration, _itemDataValidator, _seriesDataValidator, _brandDataValidator, _collectionDataValidator);
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
                _logger.LogError($"[200300003] AddItem ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300004] AddItem Exception: {e}.");
                throw;
            }
        }
    }
}