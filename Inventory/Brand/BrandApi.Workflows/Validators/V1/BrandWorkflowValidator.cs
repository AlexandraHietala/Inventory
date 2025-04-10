using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BrandApi.Data.DataOperations.V1;
using BrandApi.Models.Classes.V1;
using BrandApi.Models.System;
using BrandApi.Data.Validators.V1;

namespace BrandApi.Workflows.Validators.V1
{
    public interface IBrandWorkflowValidatorV1
    {
        Task<string> ValidateAddBrand(Brand brand);
        Task<string> ValidateUpdateBrand(Brand brand);
        Task<string> ValidateBrandId(int id);
    }

    public class BrandWorkflowValidatorV1 : IBrandWorkflowValidatorV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IBrandDataValidatorV1 _dataValidator;

        public BrandWorkflowValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration, IBrandDataValidatorV1 dataValidator)
        {
            _logger = loggerFactory.CreateLogger<BrandWorkflowValidatorV1>();
            _configuration = configuration;
            _dataValidator = dataValidator;
        }

        public async Task<string> ValidateAddBrand(Brand brand)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (brand == null)
                failureList.Add(new ValidationFailure() { Code = 300400001, Message = "Brand object is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdateBrand(Brand brand)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (brand == null)
                failureList.Add(new ValidationFailure() { Code = 300400002, Message = "Brand object is invalid." });

            if (brand != null)
            {
                bool brandExists = await ValidateBrandExists(brand.Id);
                if (!brandExists) failureList.Add(new ValidationFailure() { Code = 300400011, Message = "Brand does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateBrandId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool brandExists = await ValidateBrandExists(id);
            if (!brandExists) failureList.Add(new ValidationFailure() { Code = 300400003, Message = "Brand does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateBrandExists(int id)
        {
            bool valid = await _dataValidator.VerifyBrand(id);
            return valid;
        }
    }
}
