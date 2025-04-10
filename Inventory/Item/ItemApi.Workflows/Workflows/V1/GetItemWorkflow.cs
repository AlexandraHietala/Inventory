using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;
using ItemApi.Data.Validators.V1;
using BrandApi.Data.Validators.V1;
using SeriesApi.Data.Validators.V1;
using CollectionApi.Data.Validators.V1;

namespace ItemApi.Workflows.Workflows.V1
{
    public interface IGetItemWorkflowV1
    {
        Task<Item> GetItem(int id);
        Task<List<Item>> GetItems();

        Task<List<Item>> GetItemsPerCollection(int collectionId);
    }

    public class GetItemWorkflowV1 : IGetItemWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetItemOperationsV1 _getItemOperations;
        private readonly IItemDataValidatorV1 _itemDataValidator;
        private readonly ISeriesDataValidatorV1 _seriesDataValidator;
        private readonly IBrandDataValidatorV1 _brandDataValidator;
        private readonly ICollectionDataValidatorV1 _collectionDataValidator;
        private readonly IItemWorkflowValidatorV1 _workflowValidator;

        public GetItemWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemWorkflowV1>();
            _configuration = configuration;
            _getItemOperations = new GetItemOperationsV1(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidatorV1(loggerFactory, configuration);
            _seriesDataValidator = new SeriesDataValidatorV1(loggerFactory, configuration);
            _brandDataValidator = new BrandDataValidatorV1(loggerFactory, configuration);
            _collectionDataValidator = new CollectionDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new ItemWorkflowValidatorV1(loggerFactory, configuration, _itemDataValidator, _seriesDataValidator, _brandDataValidator, _collectionDataValidator);
        }

        public async Task<Item> GetItem(int id)
        {
            _logger.LogDebug("GetItem request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateItemId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemDto itemDto = await _getItemOperations.GetItem(id);
                Item item = ItemConverter.ConvertItemDtoToItem(itemDto);

                // Respond
                _logger.LogInformation("GetItem success response.");
                return item;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300009] GetItem ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300010] GetItem Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Item>> GetItems()
        {
            _logger.LogDebug("GetItems request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<ItemDto> itemDtos = await _getItemOperations.GetItems(null);
                List<Item> items = ItemConverter.ConvertListItemDtoToListItem(itemDtos);

                // Respond
                _logger.LogInformation("GetItems success response.");
                return items;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300011] GetItems ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300012] GetItems Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Item>> GetItemsPerCollection(int collectionId)
        {
            _logger.LogDebug("GetItemsPerCollection request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateCollectionId(collectionId);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                List<ItemDto> itemDtos = await _getItemOperations.GetItemsPerCollection(collectionId);
                List<Item> items = ItemConverter.ConvertListItemDtoToListItem(itemDtos);

                // Respond
                _logger.LogInformation("GetItemsPerCollection success response.");
                return items;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300013] GetItemsPerCollection ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300014] GetItemsPerCollection Exception: {e}.");
                throw;
            }
        }
    }
}