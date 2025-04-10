using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Asp.Versioning;
using ItemApi.Validators.V1;
using ItemApi.Workflows.Workflows.V1;
using ItemApi.Models.Classes.V1;

namespace ItemApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("GetItemControllerV1")]
    [Route("api/v{version:apiVersion}/item")]
    public class GetItemControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemControllerValidatorV1 _controllerValidator;
        private readonly IGetItemWorkflowV1 _getItemWorkflow;

        public GetItemControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemControllerV1>();
            _configuration = configuration;
            _controllerValidator = new ItemControllerValidatorV1();
            _getItemWorkflow = new GetItemWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getitem")]
        public async Task<IActionResult> GetItemV1(int id)
        {
            _logger.LogDebug("GetItem request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateItemId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Item requestedItem = await _getItemWorkflow.GetItem(id);

                // Respond
                _logger.LogInformation("GetItem success response.");
                return Ok(requestedItem);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100013] GetItem ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100014] GetItem InvalidOperationException: {ioe}.");
                return NotFound("[200100014] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100015] GetItem Exception: {e}.");
                return Problem("[200100015] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getitems")]
        public async Task<IActionResult> GetItemsV1()
        {
            _logger.LogDebug("GetItems request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<Item> requestedItems = await _getItemWorkflow.GetItems();

                // Respond
                _logger.LogInformation("GetItems success response.");
                return Ok(requestedItems);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100016] GetItems ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100017] GetItems InvalidOperationException: {ioe}.");
                return NotFound("[200100017] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100018] GetItems Exception: {e}.");
                return Problem("[200100018] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getitemspercollection")]
        public async Task<IActionResult> GetItemsPerCollectionV1(int collectionId)
        {
            _logger.LogDebug("GetItemsPerCollectionV1 request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateCollectionId(collectionId);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                List<Item> requestedItems = await _getItemWorkflow.GetItemsPerCollection(collectionId);

                // Respond
                _logger.LogInformation("GetItemsPerCollectionV1 success response.");
                return Ok(requestedItems);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100019] GetItemsPerCollectionV1 ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100020] GetItemsPerCollectionV1 InvalidOperationException: {ioe}.");
                return NotFound("[200100020] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100021] GetItemsPerCollectionV1 Exception: {e}.");
                return Problem("[200100021] " + e.Message);
            }
        }


        
    }
}
