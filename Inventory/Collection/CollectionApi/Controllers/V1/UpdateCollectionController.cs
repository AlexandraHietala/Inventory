using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CollectionApi.Models.Classes.V1;
using CollectionApi.Validators.V1;
using CollectionApi.Workflows.Workflows.V1;

namespace CollectionApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("UpdateCollectionControllerV1")]
    [Route("api/v{version:apiVersion}/collection")]
    public class UpdateCollectionControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ICollectionControllerValidatorV1 _controllerValidator;
        private readonly IUpdateCollectionWorkflowV1 _updateCollectionWorkflow;

        public UpdateCollectionControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateCollectionControllerV1>();
            _configuration = configuration;
            _controllerValidator = new CollectionControllerValidatorV1();
            _updateCollectionWorkflow = new UpdateCollectionWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updatecollection")]
        public async Task<IActionResult> UpdateCollectionV1(int id, string collectionName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateCollection request received.");

            try
            {
                // Validate
                Collection item = new Collection()
                {
                    Id = id,
                    CollectionName = collectionName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdateCollection(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateCollectionWorkflow.UpdateCollection(item);

                // Respond
                _logger.LogInformation("UpdateCollection success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500100013] UpdateCollection ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[500100014] UpdateCollection InvalidOperationException: {ioe}.");
                return NotFound("[500100014] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[500100015] UpdateCollection Exception: {e}.");
                return Problem("[500100015] " + e.Message);
            }
        }
    }
}
