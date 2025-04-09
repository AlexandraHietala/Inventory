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
        private readonly IGeneralControllerValidatorV1 _controllerValidator;
        private readonly IGetCollectionWorkflowV1 _getCollectionWorkflow;

        public GetCollectionControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetCollectionControllerV1>();
            _configuration = configuration;
            _controllerValidator = new GeneralControllerValidatorV1();
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
                var failures = _controllerValidator.ValidateId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Collection collection = await _getCollectionWorkflow.GetCollection(id);

                // Respond
                _logger.LogInformation("GetCollection success response.");
                return Ok(collection);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300100001] GetCollection ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[300100002] GetCollection InvalidOperationException: {ioe}.");
                return NotFound("[300100002] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[300100003] GetCollection Exception: {e}.");
                return Problem("[300100003] " + e.Message);
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
                _logger.LogError($"[300100004] GetCollections ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[300100005] GetCollections InvalidOperationException: {ioe}.");
                return NotFound("[300100005] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[300100006] GetCollections Exception: {e}.");
                return Problem("[300100006] " + e.Message);
            }
        }
    }
}
