using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CollectionApi.Models;
using CollectionApi.Validators.V1;
using CollectionApi.Workflows.Workflows.V1;

namespace CollectionApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("RemoveCollectionControllerV1")]
    [Route("api/v{version:apiVersion}/collection")]
    public class RemoveCollectionControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGeneralControllerValidatorV1 _controllerValidator;
        private readonly IRemoveCollectionWorkflowV1 _removeCollectionWorkflow;

        public RemoveCollectionControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveCollectionControllerV1>();
            _configuration = configuration;
            _controllerValidator = new GeneralControllerValidatorV1();
            _removeCollectionWorkflow = new RemoveCollectionWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removecollection")]
        public async Task<IActionResult> RemoveCollectionV1(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveCollection request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeCollectionWorkflow.RemoveCollection(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveCollection success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300100009] RemoveCollection ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[300100010] RemoveCollection InvalidOperationException: {ioe}.");
                return NotFound("[300100010] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[300100011] RemoveCollection Exception: {e}.");
                return Problem("[300100011] " + e.Message);
            }
        }
    }
}
