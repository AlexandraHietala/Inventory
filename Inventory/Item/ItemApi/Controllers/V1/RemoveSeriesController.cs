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
    [ControllerName("RemoveSeriesControllerV1")]
    [Route("api/v{version:apiVersion}/series")]
    public class RemoveSeriesControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGeneralControllerValidatorV1 _controllerValidator;
        private readonly IRemoveSeriesWorkflowV1 _removeSeriesWorkflow;

        public RemoveSeriesControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveSeriesControllerV1>();
            _configuration = configuration;
            _controllerValidator = new GeneralControllerValidatorV1();
            _removeSeriesWorkflow = new RemoveSeriesWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removeseries")]
        public async Task<IActionResult> RemoveSeriesV1(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveSeries request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeSeriesWorkflow.RemoveSeries(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveSeries success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100046] RemoveSeries ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100047] RemoveSeries InvalidOperationException: {ioe}.");
                return NotFound("[200100047] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100048] RemoveSeries Exception: {e}.");
                return Problem("[200100048] " + e.Message);
            }
        }
    }
}
