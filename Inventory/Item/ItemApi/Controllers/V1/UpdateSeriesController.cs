using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Models.Classes.V1;
using ItemApi.Validators.V1;
using ItemApi.Workflows.Workflows.V1;

namespace ItemApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("UpdateSeriesControllerV1")]
    [Route("api/v{version:apiVersion}/series")]
    public class UpdateSeriesControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISeriesControllerValidatorV1 _controllerValidator;
        private readonly IUpdateSeriesWorkflowV1 _updateSeriesWorkflow;

        public UpdateSeriesControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateSeriesControllerV1>();
            _configuration = configuration;
            _controllerValidator = new SeriesControllerValidatorV1();
            _updateSeriesWorkflow = new UpdateSeriesWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updateseries")]
        public async Task<IActionResult> UpdateSeriesV1(int id, string seriesName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateSeries request received.");

            try
            {
                // Validate
                Series item = new Series()
                {
                    Id = id,
                    SeriesName = seriesName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdateSeries(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateSeriesWorkflow.UpdateSeries(item);

                // Respond
                _logger.LogInformation("UpdateSeries success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100058] UpdateSeries ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100059] UpdateSeries InvalidOperationException: {ioe}.");
                return NotFound("[200100059] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100060] UpdateSeries Exception: {e}.");
                return Problem("[200100060] " + e.Message);
            }
        }
    }
}
