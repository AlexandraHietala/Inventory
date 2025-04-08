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
    [ControllerName("AddSeriesControllerV1")]
    [Route("api/v{version:apiVersion}/series")]
    public class AddSeriesControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISeriesControllerValidatorV1 _controllerValidator;
        private readonly IAddSeriesWorkflowV1 _addSeriesWorkflow;

        public AddSeriesControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddSeriesControllerV1>();
            _configuration = configuration;
            _controllerValidator = new SeriesControllerValidatorV1();
            _addSeriesWorkflow = new AddSeriesWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("addseries")]
        public async Task<IActionResult> AddSeriesV1(string seriesName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("AddSeries request received.");

            try
            {
                // Validate
                Series series = new Series()
                {
                    Id = 0,
                    SeriesName = seriesName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateAddSeries(series);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _addSeriesWorkflow.AddSeries(series);

                // Respond
                _logger.LogInformation("AddSeries success response.");
                return Ok(id);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100010] AddSeries ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100011] AddSeries InvalidOperationException: {ioe}.");
                return NotFound("[200100011] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100012] AddSeries Exception: {e}.");
                return Problem("[200100012] " + e.Message);
            }
        }
    }
}
