using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BrandApi.Data.DataOperations.V1;
using BrandApi.Models.Classes.V1;
using BrandApi.Models.Converters.V1;
using BrandApi.Models.DTOs.V1;
using BrandApi.Workflows.Validators.V1;

namespace BrandApi.Workflows.Workflows.V1
{
    public interface IAddBrandWorkflowV1
    {
        Task<int> AddBrand(Brand brand);
    }

    public class AddBrandWorkflowV1 : IAddBrandWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddBrandOperationsV1 _addBrandOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IBrandWorkflowValidatorV1 _workflowValidator;


        public AddBrandWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddBrandWorkflowV1>();
            _configuration = configuration;
            _addBrandOperations = new AddBrandOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new BrandWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task<int> AddBrand(Brand brand)
        {
            _logger.LogDebug("AddBrand request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateAddBrand(brand);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                BrandDto brandDto = BrandConverter.ConvertBrandToBrandDto(brand);
                int newBrandId = await _addBrandOperations.AddBrand(brandDto);

                // Respond
                _logger.LogInformation("AddBrand success response.");
                return newBrandId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300001] AddBrand ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300002] AddBrand Exception: {e}.");
                throw;
            }
        }
    }
}