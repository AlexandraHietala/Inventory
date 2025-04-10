using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CollectionApi.Workflows.Validators.V1;
using CollectionApi.Data.DataOperations.V1;
using CollectionApi.Models.Classes.V1;
using CollectionApi.Models.Converters.V1;
using CollectionApi.Models.DTOs.V1;
using CollectionApi.Data.Validators.V1;

namespace CollectionApi.Workflows.Workflows.V1
{
    public interface IGetCollectionWorkflowV1
    {
        Task<Collection> GetCollection(int id);
        Task<List<Collection>> GetCollections();
    }

    public class GetCollectionWorkflowV1 : IGetCollectionWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetCollectionOperationsV1 _getCollectionOperations;
        private readonly ICollectionDataValidatorV1 _dataValidator;
        private readonly ICollectionWorkflowValidatorV1 _workflowValidator;

        public GetCollectionWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetCollectionWorkflowV1>();
            _configuration = configuration;
            _getCollectionOperations = new GetCollectionOperationsV1(loggerFactory, configuration);
            _dataValidator = new CollectionDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new CollectionWorkflowValidatorV1(loggerFactory, configuration, _dataValidator);
        }

        public async Task<Collection> GetCollection(int id)
        {
            _logger.LogDebug("GetCollection request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateCollectionId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                CollectionDto collectionDto = await _getCollectionOperations.GetCollection(id);
                Collection collection = CollectionConverter.ConvertCollectionDtoToCollection(collectionDto);

                // Respond
                _logger.LogInformation("GetCollection success response.");
                return collection;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500300003] GetCollection ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[500300004] GetCollection Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Collection>> GetCollections()
        {
            _logger.LogDebug("GetCollections request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<CollectionDto> collectionDtos = await _getCollectionOperations.GetCollections(null);
                List<Collection> collections = CollectionConverter.ConvertListCollectionDtoToListCollection(collectionDtos);

                // Respond
                _logger.LogInformation("GetCollections success response.");
                return collections;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500300005] GetCollections ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[500300006] GetCollections Exception: {e}.");
                throw;
            }
        }
    }
}