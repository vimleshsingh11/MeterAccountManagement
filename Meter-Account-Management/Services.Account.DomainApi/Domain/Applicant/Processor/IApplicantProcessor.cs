using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Account.DomainApi.ViewModel;
using Services.Account.Surface;

namespace Services.Account.DomainApi.Domain.Applicant.Processor
{
    public interface IApplicantProcessor
    {
        /// <summary>
        /// Add new customer
        /// </summary>
        /// <param name="customerDetails">customer details</param>
        /// <returns></returns>
        bool AddNewApplicant(CustomerInformationCommand customerDetails);

        /// <summary>
        /// Import the applicantDetails
        /// </summary>
        /// <returns></returns>
        Task<string> UploadNewApplicant();


        /// <summary>
        /// Reterive the all applicant details
        /// </summary>
        /// <returns></returns>
        List<CustomerDetails> GetAllApplicantDetails();
    }
}
