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
    [ControllerName("GetItemCommentControllerV1")]
    [Route("api/v{version:apiVersion}/comment")]
    public class GetItemCommentControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGeneralControllerValidatorV1 _controllerValidator;
        private readonly IGetItemCommentWorkflowV1 _getItemCommentWorkflow;

        public GetItemCommentControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemCommentControllerV1>();
            _configuration = configuration;
            _controllerValidator = new GeneralControllerValidatorV1();
            _getItemCommentWorkflow = new GetItemCommentWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getitemcomment")]
        public async Task<IActionResult> GetItemCommentV1(int id)
        {
            _logger.LogDebug("GetItemComment request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemComment comment = await _getItemCommentWorkflow.GetItemComment(id);

                // Respond
                _logger.LogInformation("GetItemComment success response.");
                return Ok(comment);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100019] GetItemComment ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100020] GetItemComment InvalidOperationException: {ioe}.");
                return NotFound("[200100020] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100021] GetItemComment Exception: {e}.");
                return Problem("[200100021] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getitemcomments")]
        public async Task<IActionResult> GetItemCommentsV1(int itemId)
        {
            _logger.LogDebug("GetItemComments request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateId(itemId);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                List<ItemComment> comments = await _getItemCommentWorkflow.GetItemComments(itemId);

                // Respond
                _logger.LogInformation("GetItemComments success response.");
                return Ok(comments);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100022] GetItemComments ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100023] GetItemComments InvalidOperationException: {ioe}.");
                return NotFound("[200100023] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100024] GetItemComments Exception: {e}.");
                return Problem("[200100024] " + e.Message);
            }
        }
    }
}
