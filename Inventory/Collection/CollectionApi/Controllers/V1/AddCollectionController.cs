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
    [ControllerName("AddCollectionControllerV1")]
    [Route("api/v{version:apiVersion}/collection")]
    public class AddCollectionControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ICollectionControllerValidatorV1 _controllerValidator;
        private readonly IAddCollectionWorkflowV1 _addCollectionWorkflow;

        public AddCollectionControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddCollectionControllerV1>();
            _configuration = configuration;
            _controllerValidator = new CollectionControllerValidatorV1();
            _addCollectionWorkflow = new AddCollectionWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("addcollection")]
        public async Task<IActionResult> AddCollectionV1(string collectionName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("AddCollection request received.");

            try
            {
                // Validate
                Collection collection = new Collection()
                {
                    Id = 0,
                    CollectionName = collectionName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateAddCollection(collection);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _addCollectionWorkflow.AddCollection(collection);

                // Respond
                _logger.LogInformation("AddCollection success response.");
                return Ok(id);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500100001] AddCollection ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[500100002] AddCollection InvalidOperationException: {ioe}.");
                return NotFound("[500100002] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[500100003] AddCollection Exception: {e}.");
                return Problem("[500100003] " + e.Message);
            }
        }
    }
}
