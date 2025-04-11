using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.System;
using ItemApi.Data.Validators.V1;

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
        private readonly IItemDataValidatorV1 _itemDataValidator;

        public ItemWorkflowValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration, IItemDataValidatorV1 itemDataValidator)
        {
            _logger = loggerFactory.CreateLogger<ItemWorkflowValidatorV1>();
            _configuration = configuration;
            _itemDataValidator = itemDataValidator;
        }

        public async Task<string> ValidateAddItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200400008, Message = "Item object is invalid." });

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

        private async Task<bool> ValidateItemExists(int id)
        {
            bool valid = await _itemDataValidator.VerifyItem(id);
            return valid;
        }

    }
}
