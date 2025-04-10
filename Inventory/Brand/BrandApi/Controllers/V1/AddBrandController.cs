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
    [ControllerName("AddBrandControllerV1")]
    [Route("api/v{version:apiVersion}/brand")]
    public class AddBrandControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IBrandControllerValidatorV1 _controllerValidator;
        private readonly IAddBrandWorkflowV1 _addBrandWorkflow;

        public AddBrandControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddBrandControllerV1>();
            _configuration = configuration;
            _controllerValidator = new BrandControllerValidatorV1();
            _addBrandWorkflow = new AddBrandWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("addbrand")]
        public async Task<IActionResult> AddBrandV1(string brandName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("AddBrand request received.");

            try
            {
                // Validate
                Brand brand = new Brand()
                {
                    Id = 0,
                    BrandName = brandName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateAddBrand(brand);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _addBrandWorkflow.AddBrand(brand);

                // Respond
                _logger.LogInformation("AddBrand success response.");
                return Ok(id);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100001] AddBrand ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100002] AddBrand InvalidOperationException: {ioe}.");
                return NotFound("[200100002] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100003] AddBrand Exception: {e}.");
                return Problem("[200100003] " + e.Message);
            }
        }
    }
}
