using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Workflows.Workflows.V1
{
    public interface IUpdateBrandWorkflowV1
    {
        Task UpdateBrand(Brand brand);
    }

    public class UpdateBrandWorkflowV1 : IUpdateBrandWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateBrandOperationsV1 _updateBrandOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IBrandWorkflowValidatorV1 _workflowValidator;

        public UpdateBrandWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateBrandWorkflowV1>();
            _configuration = configuration;
            _updateBrandOperations = new UpdateBrandOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new BrandWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task UpdateBrand(Brand brand)
        {
            _logger.LogDebug("UpdateBrand request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateUpdateBrand(brand);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                BrandDto brandDto = BrandConverter.ConvertBrandToBrandDto(brand);
                await _updateBrandOperations.UpdateBrand(brandDto);

                // Respond
                _logger.LogInformation("UpdateBrand success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300033] UpdateBrand ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300034] UpdateBrand Exception: {e}.");
                throw;
            }
        }
    }
}