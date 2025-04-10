using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.System;
using ItemApi.Data.Validators.V1;
using SeriesApi.Data.Validators.V1;
using BrandApi.Data.Validators.V1;
using CollectionApi.Data.Validators.V1;

namespace ItemApi.Workflows.Validators.V1
{
    public interface IItemWorkflowValidatorV1
    {
        Task<string> ValidateAddItem(Item item);
        Task<string> ValidateUpdateItem(Item item);
        Task<string> ValidateItemId(int id);
        Task<string> ValidateCollectionId(int id);
    }

    public class ItemWorkflowValidatorV1 : IItemWorkflowValidatorV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemDataValidatorV1 _itemDataValidator;
        private readonly ISeriesDataValidatorV1 _seriesDataValidator;
        private readonly IBrandDataValidatorV1 _brandDataValidator;
        private readonly ICollectionDataValidatorV1 _collectionDataValidator;

        public ItemWorkflowValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration, IItemDataValidatorV1 itemDataValidator, ISeriesDataValidatorV1 seriesDataValidator, IBrandDataValidatorV1 brandDataValidator, ICollectionDataValidatorV1 collectionDataValidator)
        {
            _logger = loggerFactory.CreateLogger<ItemWorkflowValidatorV1>();
            _configuration = configuration;
            _itemDataValidator = itemDataValidator;
            _seriesDataValidator = seriesDataValidator;
            _brandDataValidator = brandDataValidator;
            _collectionDataValidator = collectionDataValidator;
        }

        public async Task<string> ValidateAddItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200400008, Message = "Item object is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId.HasValue)
            {
                bool seriesExists = await ValidateSeriesExists(item.SeriesId.Value);
                if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 200400009, Message = "Series does not exist." });
            }

            if (item != null && item.BrandId != null && item.BrandId.HasValue)
            {
                bool brandExists = await ValidateBrandExists(item.BrandId.Value);
                if (!brandExists) failureList.Add(new ValidationFailure() { Code = 200400010, Message = "Brand does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdateItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200400011, Message = "Item object is invalid." });

            if (item != null)
            {
                bool itemExists = await ValidateItemExists(item.Id);
                if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400012, Message = "Item does not exist." });
            }

            if (item != null && item.SeriesId != null && item.SeriesId.HasValue)
            {
                bool seriesExists = await ValidateSeriesExists(item.SeriesId.Value);
                if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 200400013, Message = "Series does not exist." });
            }

            if (item != null && item.BrandId != null && item.BrandId.HasValue)
            {
                bool brandExists = await ValidateBrandExists(item.BrandId.Value);
                if (!brandExists) failureList.Add(new ValidationFailure() { Code = 200400014, Message = "Brand does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateItemId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool itemExists = await ValidateItemExists(id);
            if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400015, Message = "Item does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateCollectionId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool itemExists = await ValidateCollectionExists(id);
            if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400016, Message = "Collection does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateItemExists(int id)
        {
            bool valid = await _itemDataValidator.VerifyItem(id);
            return valid;
        }

        private async Task<bool> ValidateSeriesExists(int id)
        {
            bool valid = await _seriesDataValidator.VerifySeries(id);
            return valid;
        }

        private async Task<bool> ValidateBrandExists(int id)
        {
            bool valid = await _brandDataValidator.VerifyBrand(id);
            return valid;
        }

        private async Task<bool> ValidateCollectionExists(int id)
        {
            bool valid = await _collectionDataValidator.VerifyCollection(id);
            return valid;
        }
    }
}
