using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CollectionApi.Data.DataOperations.V1;
using CollectionApi.Models.Classes.V1;
using CollectionApi.Models.System;

namespace CollectionApi.Workflows.Validators.V1
{
    public interface ICollectionWorkflowValidatorV1
    {
        Task<string> ValidateAddCollection(Collection collection);
        Task<string> ValidateUpdateCollection(Collection collection);
        Task<string> ValidateCollectionId(int id);
    }

    public class CollectionWorkflowValidatorV1 : ICollectionWorkflowValidatorV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IVerifyOperationsV1 _verifyOperations;

        public CollectionWorkflowValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration, IVerifyOperationsV1 verifyOperations)
        {
            _logger = loggerFactory.CreateLogger<CollectionWorkflowValidatorV1>();
            _configuration = configuration;
            _verifyOperations = verifyOperations;
        }

        public async Task<string> ValidateAddCollection(Collection collection)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (collection == null)
                failureList.Add(new ValidationFailure() { Code = 300400001, Message = "Collection object is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdateCollection(Collection collection)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (collection == null)
                failureList.Add(new ValidationFailure() { Code = 300400002, Message = "Collection object is invalid." });

            if (collection != null)
            {
                bool collectionExists = await ValidateCollectionExists(collection.Id);
                if (!collectionExists) failureList.Add(new ValidationFailure() { Code = 300400003, Message = "Collection does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateCollectionId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool collectionExists = await ValidateCollectionExists(id);
            if (!collectionExists) failureList.Add(new ValidationFailure() { Code = 300400004, Message = "Collection does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateCollectionExists(int id)
        {
            bool valid = await _verifyOperations.VerifyCollection(id);
            return valid;
        }
    }
}
