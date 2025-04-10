using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BrandApi.Models;
using BrandApi.Validators.V1;
using BrandApi.Workflows.Workflows.V1;

namespace BrandApi.Controllers.V1
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
        private readonly IBrandControllerValidatorV1 _controllerValidator;
        private readonly IRemoveBrandWorkflowV1 _removeBrandWorkflow;

        public RemoveBrandControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveBrandControllerV1>();
            _configuration = configuration;
            _controllerValidator = new BrandControllerValidatorV1();
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
                var failures = _controllerValidator.ValidateBrandId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeBrandWorkflow.RemoveBrand(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveBrand success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300100010] RemoveBrand ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[300100011] RemoveBrand InvalidOperationException: {ioe}.");
                return NotFound("[300100011] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[300100012] RemoveBrand Exception: {e}.");
                return Problem("[300100012] " + e.Message);
            }
        }
    }
}
