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
    [ControllerName("UpdateItemControllerV1")]
    [Route("api/v{version:apiVersion}/item")]
    public class UpdateItemControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemControllerValidatorV1 _controllerValidator;
        private readonly IUpdateItemWorkflowV1 _updateItemWorkflow;

        public UpdateItemControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemControllerV1>();
            _configuration = configuration;
            _controllerValidator = new ItemControllerValidatorV1();
            _updateItemWorkflow = new UpdateItemWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updateitem")]
        public async Task<IActionResult> UpdateItemV1(int id, string status, string type, string? brand, string? series, string? name, string? description, string format, string size, int? year, string? photo, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateItem request received.");

            try
            {
                // Validate
                Item item = new Item()
                {
                    Id = id,
                    Status = status,
                    Type = type,
                    Brand = brand,
                    Series = series,
                    Name = name,
                    Description = description,
                    Format = format,
                    Size = size,
                    Year = year,
                    Photo = photo,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdateItem(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateItemWorkflow.UpdateItem(item);

                // Respond
                _logger.LogInformation("UpdateItem success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100031] UpdateItem ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100032] UpdateItem InvalidOperationException: {ioe}.");
                return NotFound("[200100032] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100033] UpdateItem Exception: {e}.");
                return Problem("[200100033] " + e.Message);
            }
        }
    }
}
