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
    [ControllerName("RemoveItemCommentControllerV1")]
    [Route("api/v{version:apiVersion}/comment")]
    public class RemoveItemCommentControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGeneralControllerValidatorV1 _controllerValidator;
        private readonly IRemoveItemCommentWorkflowV1 _removeItemCommentWorkflow;

        public RemoveItemCommentControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemCommentControllerV1>();
            _configuration = configuration;
            _controllerValidator = new GeneralControllerValidatorV1();
            _removeItemCommentWorkflow = new RemoveItemCommentWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removeitemcomment")]
        public async Task<IActionResult> RemoveItemCommentV1(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveItemComment request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeItemCommentWorkflow.RemoveItemComment(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveItemComment success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100040] RemoveItemComment ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100041] RemoveItemComment InvalidOperationException: {ioe}.");
                return NotFound("[200100041] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100042] RemoveItemComment Exception: {e}.");
                return Problem("[200100042] " + e.Message);
            }
        }
    }
}
