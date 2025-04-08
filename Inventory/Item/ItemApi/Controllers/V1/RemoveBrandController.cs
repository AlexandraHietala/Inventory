using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Models;
using ItemApi.Validators.V1;
using ItemApi.Workflows.Workflows.V1;

namespace ItemApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("RemoveBrandControllerV1")]
    [Route("api/v{version:apiVersion}/brand")]
    public class RemoveBrandControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGeneralControllerValidatorV1 _controllerValidator;
        private readonly IRemoveBrandWorkflowV1 _removeBrandWorkflow;

        public RemoveBrandControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveBrandControllerV1>();
            _configuration = configuration;
            _controllerValidator = new GeneralControllerValidatorV1();
            _removeBrandWorkflow = new RemoveBrandWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removebrand")]
        public async Task<IActionResult> RemoveBrandV1(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveBrand request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeBrandWorkflow.RemoveBrand(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveBrand success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100037] RemoveBrand ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100038] RemoveBrand InvalidOperationException: {ioe}.");
                return NotFound("[200100038] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100039] RemoveBrand Exception: {e}.");
                return Problem("[200100039] " + e.Message);
            }
        }
    }
}
