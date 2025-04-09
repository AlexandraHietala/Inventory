using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CollectionApi.Workflows.Validators.V1;
using CollectionApi.Data.DataOperations.V1;
using CollectionApi.Models.Classes.V1;
using CollectionApi.Models.Converters.V1;
using CollectionApi.Models.DTOs.V1;

namespace CollectionApi.Workflows.Workflows.V1
{
    public interface IUpdateCollectionWorkflowV1
    {
        Task UpdateCollection(Collection collection);
    }

    public class UpdateCollectionWorkflowV1 : IUpdateCollectionWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateCollectionOperationsV1 _updateCollectionOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly ICollectionWorkflowValidatorV1 _workflowValidator;

        public UpdateCollectionWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateCollectionWorkflowV1>();
            _configuration = configuration;
            _updateCollectionOperations = new UpdateCollectionOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new CollectionWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task UpdateCollection(Collection collection)
        {
            _logger.LogDebug("UpdateCollection request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateUpdateCollection(collection);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                CollectionDto collectionDto = CollectionConverter.ConvertCollectionToCollectionDto(collection);
                await _updateCollectionOperations.UpdateCollection(collectionDto);

                // Respond
                _logger.LogInformation("UpdateCollection success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300300009] UpdateCollection ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[300300010] UpdateCollection Exception: {e}.");
                throw;
            }
        }
    }
}