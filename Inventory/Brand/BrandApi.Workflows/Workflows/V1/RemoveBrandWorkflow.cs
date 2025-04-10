using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BrandApi.Data.DataOperations.V1;
using BrandApi.Workflows.Validators.V1;

namespace BrandApi.Workflows.Workflows.V1
{
    public interface IRemoveBrandWorkflowV1
    {
        Task RemoveBrand(int id, string lastmodifiedby);
    }

    public class RemoveBrandWorkflowV1 : IRemoveBrandWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveBrandOperationsV1 _removeBrandOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IBrandWorkflowValidatorV1 _workflowValidator;

        public RemoveBrandWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveBrandWorkflowV1>();
            _configuration = configuration;
            _removeBrandOperations = new RemoveBrandOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new BrandWorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task RemoveBrand(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveBrand request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateBrandId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeBrandOperations.RemoveBrand(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveBrand success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300025] RemoveBrand ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300026] RemoveBrand Exception: {e}.");
                throw;
            }
        }
    }
}