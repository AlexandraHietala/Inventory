using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.System;

namespace ItemApi.Workflows.Validators.V1
{
    public interface IItemWorkflowValidatorV1
    {
        Task<string> ValidateAddItem(Item item);
        Task<string> ValidateUpdateItem(Item item);
        Task<string> ValidateItemId(int id);
    }

    public class ItemWorkflowValidatorV1 : IItemWorkflowValidatorV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IVerifyOperationsV1 _verifyOperations;

        public ItemWorkflowValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration, IVerifyOperationsV1 verifyOperations)
        {
            _logger = loggerFactory.CreateLogger<ItemWorkflowValidatorV1>();
            _configuration = configuration;
            _verifyOperations = verifyOperations;
        }

        public async Task<string> ValidateAddItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200400001, Message = "Item object is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId.HasValue)
            {
                bool seriesExists = await ValidateSeriesExists(item.SeriesId.Value);
                if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 200400002, Message = "Series does not exist." });
            }

            if (item != null && item.BrandId != null && item.BrandId.HasValue)
            {
                bool brandExists = await ValidateBrandExists(item.BrandId.Value);
                if (!brandExists) failureList.Add(new ValidationFailure() { Code = 200400003, Message = "Brand does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdateItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200400004, Message = "Item object is invalid." });

            if (item != null)
            {
                bool itemExists = await ValidateItemExists(item.Id);
                if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400005, Message = "Item does not exist." });
            }

            if (item != null && item.SeriesId != null && item.SeriesId.HasValue)
            {
                bool seriesExists = await ValidateSeriesExists(item.SeriesId.Value);
                if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 200400006, Message = "Series does not exist." });
            }

            if (item != null && item.BrandId != null && item.BrandId.HasValue)
            {
                bool brandExists = await ValidateBrandExists(item.BrandId.Value);
                if (!brandExists) failureList.Add(new ValidationFailure() { Code = 200400007, Message = "Brand does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateItemId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool itemExists = await ValidateItemExists(id);
            if (!itemExists) failureList.Add(new ValidationFailure() { Code = 200400008, Message = "Item does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateItemExists(int id)
        {
            bool valid = await _verifyOperations.VerifyItem(id);
            return valid;
        }

        private async Task<bool> ValidateSeriesExists(int id)
        {
            bool valid = await _verifyOperations.VerifySeries(id);
            return valid;
        }

        private async Task<bool> ValidateBrandExists(int id)
        {
            bool valid = await _verifyOperations.VerifyBrand(id);
            return valid;
        }
    }
}
