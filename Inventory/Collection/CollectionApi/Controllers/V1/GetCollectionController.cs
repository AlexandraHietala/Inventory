using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Asp.Versioning;
using CollectionApi.Validators.V1;
using CollectionApi.Workflows.Workflows.V1;
using CollectionApi.Models.Classes.V1;

namespace CollectionApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("GetCollectionControllerV1")]
    [Route("api/v{version:apiVersion}/collection")]
    public class GetCollectionControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ICollectionControllerValidatorV1 _controllerValidator;
        private readonly IGetCollectionWorkflowV1 _getCollectionWorkflow;

        public GetCollectionControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetCollectionControllerV1>();
            _configuration = configuration;
            _controllerValidator = new CollectionControllerValidatorV1();
            _getCollectionWorkflow = new GetCollectionWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getcollection")]
        public async Task<IActionResult> GetCollectionV1(int id)
        {
            _logger.LogDebug("GetCollection request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateCollectionId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Collection collection = await _getCollectionWorkflow.GetCollection(id);

                // Respond
                _logger.LogInformation("GetCollection success response.");
                return Ok(collection);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500100004] GetCollection ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[500100005] GetCollection InvalidOperationException: {ioe}.");
                return NotFound("[500100005] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[500100006] GetCollection Exception: {e}.");
                return Problem("[500100006] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getcollections")]
        public async Task<IActionResult> GetCollectionsV1()
        {
            _logger.LogDebug("GetCollections request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<Collection> collections = await _getCollectionWorkflow.GetCollections();

                // Respond
                _logger.LogInformation("GetCollections success response.");
                return Ok(collections);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500100007] GetCollections ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[500100008] GetCollections InvalidOperationException: {ioe}.");
                return NotFound("[500100008] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[500100009] GetCollections Exception: {e}.");
                return Problem("[500100009] " + e.Message);
            }
        }
    }
}
