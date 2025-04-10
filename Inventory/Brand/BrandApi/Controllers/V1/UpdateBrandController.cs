using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BrandApi.Models.Classes.V1;
using BrandApi.Validators.V1;
using BrandApi.Workflows.Workflows.V1;

namespace BrandApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("UpdateBrandControllerV1")]
    [Route("api/v{version:apiVersion}/brand")]
    public class UpdateBrandControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IBrandControllerValidatorV1 _controllerValidator;
        private readonly IUpdateBrandWorkflowV1 _updateBrandWorkflow;

        public UpdateBrandControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateBrandControllerV1>();
            _configuration = configuration;
            _controllerValidator = new BrandControllerValidatorV1();
            _updateBrandWorkflow = new UpdateBrandWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updatebrand")]
        public async Task<IActionResult> UpdateBrandV1(int id, string brandName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateBrand request received.");

            try
            {
                // Validate
                Brand item = new Brand()
                {
                    Id = id,
                    BrandName = brandName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdateBrand(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateBrandWorkflow.UpdateBrand(item);

                // Respond
                _logger.LogInformation("UpdateBrand success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300100013] UpdateBrand ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[300100014] UpdateBrand InvalidOperationException: {ioe}.");
                return NotFound("[300100014] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[300100015] UpdateBrand Exception: {e}.");
                return Problem("[300100015] " + e.Message);
            }
        }
    }
}
