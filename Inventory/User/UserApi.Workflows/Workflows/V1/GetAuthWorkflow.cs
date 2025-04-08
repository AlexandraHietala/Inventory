using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserApi.Workflows.Validators.V1;
using UserApi.Data.DataOperations.V1;
using UserApi.Models.Classes.V1;
using UserApi.Models.Converters.V1;
using UserApi.Models.DTOs.V1;

namespace UserApi.Workflows.Workflows.V1
{
    public interface IGetAuthWorkflowV1
    {
        Task<Auth> GetAuth(int id);
    }

    public class GetAuthWorkflowV1 : IGetAuthWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetAuthOperationsV1 _authOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IWorkflowValidatorV1 _workflowValidator;

        public GetAuthWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetAuthWorkflowV1>();
            _configuration = configuration;
            _authOperations = new GetAuthOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new WorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task<Auth> GetAuth(int id)
        {
            _logger.LogDebug("GetAuth request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateUserId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                AuthDto authDto = await _authOperations.GetAuth(id);
                Auth auth = AuthConverter.ConvertAuthDtoToAuth(authDto);

                // Respond
                _logger.LogInformation("GetAuth success response.");
                return auth;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300003] GetAuth ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300004] GetAuth Exception: {e}.");
                throw;
            }
        }

    }
}