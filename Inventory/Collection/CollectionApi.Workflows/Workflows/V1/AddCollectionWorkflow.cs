using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CollectionApi.Data.DataOperations.V1;
using CollectionApi.Models.Classes.V1;
using CollectionApi.Models.Converters.V1;
using CollectionApi.Models.DTOs.V1;
using CollectionApi.Workflows.Validators.V1;
using CollectionApi.Data.Validators.V1;

namespace CollectionApi.Workflows.Workflows.V1
{
    public interface IAddCollectionWorkflowV1
    {
        Task<int> AddCollection(Collection collection);
    }

    public class AddCollectionWorkflowV1 : IAddCollectionWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddCollectionOperationsV1 _addCollectionOperations;
        private readonly ICollectionDataValidatorV1 _dataValidator;
        private readonly ICollectionWorkflowValidatorV1 _workflowValidator;


        public AddCollectionWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddCollectionWorkflowV1>();
            _configuration = configuration;
            _addCollectionOperations = new AddCollectionOperationsV1(loggerFactory, configuration);
            _dataValidator = new CollectionDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new CollectionWorkflowValidatorV1(loggerFactory, configuration, _dataValidator);
        }

        public async Task<int> AddCollection(Collection collection)
        {
            _logger.LogDebug("AddCollection request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateAddCollection(collection);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                CollectionDto collectionDto = CollectionConverter.ConvertCollectionToCollectionDto(collection);
                int newCollectionId = await _addCollectionOperations.AddCollection(collectionDto);

                // Respond
                _logger.LogInformation("AddCollection success response.");
                return newCollectionId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500300001] AddCollection ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[500300002] AddCollection Exception: {e}.");
                throw;
            }
        }
    }
}