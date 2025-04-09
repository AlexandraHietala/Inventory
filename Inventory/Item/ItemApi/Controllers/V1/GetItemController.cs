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
        private readonly IGeneralControllerValidatorV1 _controllerValidator;
        private readonly IGetItemWorkflowV1 _getItemWorkflow;

        public GetItemControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemControllerV1>();
            _configuration = configuration;
            _controllerValidator = new GeneralControllerValidatorV1();
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
                var failures = _controllerValidator.ValidateId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Item requestedItem = await _getItemWorkflow.GetItem(id);

                // Respond
                _logger.LogInformation("GetItem success response.");
                return Ok(requestedItem);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100025] GetItem ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100026] GetItem InvalidOperationException: {ioe}.");
                return NotFound("[200100026] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100027] GetItem Exception: {e}.");
                return Problem("[200100027] " + e.Message);
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
                _logger.LogError($"[200100028] GetItems ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100029] GetItems InvalidOperationException: {ioe}.");
                return NotFound("[200100029] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100030] GetItems Exception: {e}.");
                return Problem("[200100030] " + e.Message);
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
                var failures = _controllerValidator.ValidateId(collectionId);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                List<Item> requestedItems = await _getItemWorkflow.GetItemsPerCollection(collectionId);

                // Respond
                _logger.LogInformation("GetItemsPerCollectionV1 success response.");
                return Ok(requestedItems);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100061] GetItemsPerCollectionV1 ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100062] GetItemsPerCollectionV1 InvalidOperationException: {ioe}.");
                return NotFound("[200100062] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100063] GetItemsPerCollectionV1 Exception: {e}.");
                return Problem("[200100063] " + e.Message);
            }
        }


        
    }
}
