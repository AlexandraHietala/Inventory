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
    [ControllerName("AddItemControllerV1")]
    [Route("api/v{version:apiVersion}/item")]
    public class AddItemControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemControllerValidatorV1 _controllerValidator;
        private readonly IAddItemWorkflowV1 _addItemWorkflow;

        public AddItemControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemControllerV1>();
            _configuration = configuration;
            _controllerValidator = new ItemControllerValidatorV1();
            _addItemWorkflow = new AddItemWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("additem")]
        public async Task<IActionResult> AddItemV1(int collectionId, string status, string type, int? brandId, int? seriesId, string? name, string? description, string format, string size, int? year, string? photo, string lastmodifiedby)
        {
            _logger.LogDebug("AddItem request received.");
            // TODO: add storage location info, sku, barcode
            // TODO: add poly retry logic/timeouts
            try
            {
                // Validate
                Item item = new Item()
                {
                    Id = 0,
                    CollectionId = collectionId,
                    Status = status,
                    Type = type,
                    BrandId = brandId,
                    SeriesId = seriesId,
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

                var failures = _controllerValidator.ValidateAddItem(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _addItemWorkflow.AddItem(item);

                // Respond
                _logger.LogInformation("AddItem success response.");
                return Ok(id);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100004] AddItem ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100005] AddItem InvalidOperationException: {ioe}.");
                return NotFound("[200100005] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100006] AddItem Exception: {e}.");
                return Problem("[200100006] " + e.Message);
            }
        }
    }
}
