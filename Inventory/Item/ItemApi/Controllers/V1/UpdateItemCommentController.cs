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
    [ControllerName("UpdateItemCommentControllerV1")]
    [Route("api/v{version:apiVersion}/comment")]
    public class UpdateItemCommentControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemCommentControllerValidatorV1 _controllerValidator;
        private readonly IUpdateItemCommentWorkflowV1 _updateItemCommentWorkflow;

        public UpdateItemCommentControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemCommentControllerV1>();
            _configuration = configuration;
            _controllerValidator = new ItemCommentControllerValidatorV1();
            _updateItemCommentWorkflow = new UpdateItemCommentWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updateitemcomment")]
        public async Task<IActionResult> UpdateItemCommentV1(int id, int itemId, string comment, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateItemComment request received.");

            try
            {
                // Validate
                ItemComment item = new ItemComment()
                {
                    Id = id,
                    ItemId = itemId,
                    Comment = comment,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdateItemComment(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateItemCommentWorkflow.UpdateItemComment(item);

                // Respond
                _logger.LogInformation("UpdateItemComment success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100028] UpdateItemComment ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100029] UpdateItemComment InvalidOperationException: {ioe}.");
                return NotFound("[200100029] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100030] UpdateItemComment Exception: {e}.");
                return Problem("[200100030] " + e.Message);
            }
        }
    }
}
