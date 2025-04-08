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
    [ControllerName("RemoveItemControllerV1")]
    [Route("api/v{version:apiVersion}/item")]
    public class RemoveItemControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGeneralControllerValidatorV1 _controllerValidator;
        private readonly IRemoveItemWorkflowV1 _removeItemWorkflow;

        public RemoveItemControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemControllerV1>();
            _configuration = configuration;
            _controllerValidator = new GeneralControllerValidatorV1();
            _removeItemWorkflow = new RemoveItemWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removeitem")]
        public async Task<IActionResult> RemoveItemV1(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveItem request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeItemWorkflow.RemoveItem(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveItem success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100043] RemoveItem ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100044] RemoveItem InvalidOperationException: {ioe}.");
                return NotFound("[200100044] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100045] RemoveItem Exception: {e}.");
                return Problem("[200100045] " + e.Message);
            }
        }
    }
}
