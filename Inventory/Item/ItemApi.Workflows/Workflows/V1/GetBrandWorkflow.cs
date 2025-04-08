using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Workflows.Validators.V1;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.Converters.V1;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Workflows.Workflows.V1
{
    public interface IGetBrandWorkflowV1
    {
        Task<Brand> GetBrand(int id);
        Task<List<Brand>> GetBrands();
    }

    public class GetBrandWorkflowV1 : IGetBrandWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetBrandOperationsV1 _getBrandOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IBrandWorkflowValidatorV1 _workflowValidator;

        public GetBrandWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetBrandWorkflowV1>();
            _configuration = configuration;
            _getBrandOperations = new GetBrandOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new BrandWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task<Brand> GetBrand(int id)
        {
            _logger.LogDebug("GetBrand request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateBrandId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                BrandDto brandDto = await _getBrandOperations.GetBrand(id);
                Brand brand = BrandConverter.ConvertBrandDtoToBrand(brandDto);

                // Respond
                _logger.LogInformation("GetBrand success response.");
                return brand;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300009] GetBrand ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300010] GetBrand Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Brand>> GetBrands()
        {
            _logger.LogDebug("GetBrands request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<BrandDto> brandDtos = await _getBrandOperations.GetBrands(null);
                List<Brand> brands = BrandConverter.ConvertListBrandDtoToListBrand(brandDtos);

                // Respond
                _logger.LogInformation("GetBrands success response.");
                return brands;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300011] GetBrands ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300012] GetBrands Exception: {e}.");
                throw;
            }
        }
    }
}