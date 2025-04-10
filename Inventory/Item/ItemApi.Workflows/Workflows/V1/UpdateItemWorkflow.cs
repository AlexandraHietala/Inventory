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
    public interface IUpdateItemWorkflowV1
    {
        Task UpdateItem(Item item);
    }

    public class UpdateItemWorkflowV1 : IUpdateItemWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateItemOperationsV1 _updateItemOperations;
        private readonly IItemDataValidatorV1 _itemDataValidator;
        private readonly ISeriesDataValidatorV1 _seriesDataValidator;
        private readonly IBrandDataValidatorV1 _brandDataValidator;
        private readonly ICollectionDataValidatorV1 _collectionDataValidator;
        private readonly IItemWorkflowValidatorV1 _workflowValidator;

        public UpdateItemWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemWorkflowV1>();
            _configuration = configuration;
            _updateItemOperations = new UpdateItemOperationsV1(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidatorV1(loggerFactory, configuration);
            _seriesDataValidator = new SeriesDataValidatorV1(loggerFactory, configuration);
            _brandDataValidator = new BrandDataValidatorV1(loggerFactory, configuration);
            _collectionDataValidator =  new CollectionDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new ItemWorkflowValidatorV1(loggerFactory, configuration, _itemDataValidator, _seriesDataValidator, _brandDataValidator, _collectionDataValidator);
        }

        public async Task UpdateItem(Item item)
        {
            _logger.LogDebug("UpdateItem request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateUpdateItem(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemDto itemDto = ItemConverter.ConvertItemToItemDto(item);
                await _updateItemOperations.UpdateItem(itemDto);

                // Respond
                _logger.LogInformation("UpdateItem success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300021] UpdateItem ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300022] UpdateItem Exception: {e}.");
                throw;
            }
        }
    }
}