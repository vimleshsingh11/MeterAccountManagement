using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Account.Common.Exceptions;
using Services.Account.DomainApi.Domain.Applicant.Processor;
using Services.Account.DomainApi.ViewModel;
using Services.Account.Surface;

namespace Services.Account.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicantController : Controller
    {
        /// <summary>
        /// Logger object for Controller
        /// </summary>
        private ILogger<ApplicantController> _logger { get; set; }

        /// <summary>
        ///Domain processor
        /// </summary>
        private IApplicantProcessor _applicationProcessor { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="applicantProcessor"></param>
        public ApplicantController(ILogger<ApplicantController> logger, IApplicantProcessor applicantProcessor)
        {
            _logger = logger;
            _applicationProcessor = applicantProcessor;
        }


        /// <summary>
        /// Returns the list of customer details
        /// </summary>
        /// <returns></returns>
        [HttpGet("getcustomerdetails")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [ProducesResponseType(typeof(List<CustomerDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionPayload), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllApplicantDetails()
        {
            List<CustomerDetails> customerDetails = null;
            try
            {
                _logger.LogInformation($"{nameof(GetAllApplicantDetails)} processing has been started");
                customerDetails = _applicationProcessor.GetAllApplicantDetails();
                _logger.LogInformation($"{nameof(GetAllApplicantDetails)} processing has been completed");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"{ex.GetType().Name}: Error occurred during GetAllApplicantDetails call");
                throw;
            }
            return base.Ok(new List<CustomerDetails>(customerDetails));
        }

        /// <summary>
        /// Import the customer details
        /// </summary>
        /// <returns></returns>
        [HttpPost("importcustomerdetails")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ImportCustomerDetails()
        {
            string result = string.Empty;
            try
            {
                _logger.LogInformation($"{nameof(ImportCustomerDetails)} processing has been started");
                 result =  await _applicationProcessor.UploadNewApplicant();
                _logger.LogInformation($"{nameof(ImportCustomerDetails)} processing has been completed");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"{ex.GetType().Name}: Error occurred during ImportCustomerDetails call");
                throw;
            }

            return Content(result);
        }

        /// <summary>
        /// Post the customer details
        /// </summary>
        /// <param name="customerDetails"></param>
        /// <returns></returns>
        [HttpPost("postcustomerdetails")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionPayload), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        public IActionResult PostCustomerDetails([FromBody] CustomerInformationCommand customerDetails)
        {
            try
            {
                _logger.LogInformation($"{nameof(PostCustomerDetails)} processing has been started");
                _applicationProcessor.AddNewApplicant(customerDetails);
                _logger.LogInformation($"{nameof(PostCustomerDetails)} processing has been completed");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"{ex.GetType().Name}: Error occurred during PostCustomerDetails call");
                throw;
            }
            return Ok();
        }
    }
}
