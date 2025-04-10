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
    [ControllerName("AddItemCommentControllerV1")]
    [Route("api/v{version:apiVersion}/comment")]
    public class AddItemCommentControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemCommentControllerValidatorV1 _controllerValidator;
        private readonly IAddItemCommentWorkflowV1 _addItemCommentWorkflow;

        public AddItemCommentControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemCommentControllerV1>();
            _configuration = configuration;
            _controllerValidator = new ItemCommentControllerValidatorV1();
            _addItemCommentWorkflow = new AddItemCommentWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("additemcomment")]
        public async Task<IActionResult> AddItemCommentV1(int itemId, string comment, string lastmodifiedby)
        {
            _logger.LogDebug("AddItemComment request received.");
            
            try
            {
                // Validate
                ItemComment item = new ItemComment()
                {
                    Id = 0,
                    ItemId = itemId,
                    Comment = comment,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateAddItemComment(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _addItemCommentWorkflow.AddItemComment(item);

                // Respond
                _logger.LogInformation("AddItemComment success response.");
                return Ok(id);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100001] AddItemComment ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100002] AddItemComment InvalidOperationException: {ioe}.");
                return NotFound("[200100002] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100003] AddItemComment Exception: {e}.");
                return Problem("[200100003] " + e.Message);
            }
        }
    }
}
