using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Account.DomainApi.Model;
using Services.Account.DomainApi.ViewModel;
using Services.Account.Repository.DomainRepository;
using Services.Account.Surface;

namespace Services.Account.DomainApi.Domain.Applicant.Processor
{
    public class ApplicantProcessor : IApplicantProcessor
    {
        /// <summary>
        /// Logger object for applicantprocessor
        /// </summary>
        private ILogger<ApplicantProcessor> _logger { get; set; }

        /// <summary>
        /// Repository instance
        /// </summary>
        private readonly IMongoRepository<CustomerDetails> _customerRepository;

        /// <summary>
        /// An instance of hosting environment
        /// </summary>
        private readonly IWebHostEnvironment _hostingEnv;

        /// <summary>
        /// 
        /// </summary>
        private readonly AppSettings _appSettings;


        /// <summary>
        /// 
        /// </summary>
        readonly IFileSystem _fileSystem;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerRepository"></param>
        public ApplicantProcessor(ILogger<ApplicantProcessor> logger, IOptions<AppSettings> appSettings, IWebHostEnvironment hostingEnv, IFileSystem fileSystem, IMongoRepository<CustomerDetails> customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _hostingEnv = hostingEnv;
            _fileSystem = fileSystem;
            _appSettings = appSettings.Value;
        }
        public bool AddNewApplicant(CustomerInformationCommand customerDetails)
        {
            var mapCustomerDetails = MapCustomerDetails(customerDetails);
            if(IsRecordAlredayExists(customerDetails.AccountId))
            {
                 _customerRepository.InsertOneAsync(mapCustomerDetails);
            }
            else
            {
                throw new System.Exception("Duplicate Account Id");
            }
          
            return true;
        }

        public async Task<string> UploadNewApplicant()
        {
            List<CustomerDetails> customerDetails = new List<CustomerDetails>();
            int duplicateRecord =0, newRecords = 0;

            var sourceFilePath = _hostingEnv.ContentRootPath + _fileSystem.Path.DirectorySeparatorChar + _appSettings.TestAccountFilePath;

            if (_fileSystem.File.Exists(sourceFilePath))
            {
                // var productDataJsonString = _fileSystem.File.ReadAllText(sourceFilePath);
                var result = from line in File.ReadAllLines(sourceFilePath).Skip(1)
                             let columns = line.Split(',')
                             select new
                             {
                                 AccountId = columns[0],
                                 FirstName = columns[1],
                                 LastName = columns[2]
                             };

                // check if record alreday exist
                foreach (var data in result)
                {
                    if (IsRecordAlredayExists(data.AccountId))
                    {
                        customerDetails.Add(new CustomerDetails()
                        {
                            AccountId = Convert.ToInt16(data.AccountId),
                            FirstName = data.FirstName,
                            LastName = data.LastName
                        });
                       ++ newRecords;
                    }
                    else
                    {
                        ++duplicateRecord;
                    }
                }

                if(customerDetails.Count() > 0 )
                await _customerRepository.InsertManyAsync(customerDetails);
              
            }
            return $"{newRecords} has beed added and {duplicateRecord} records are duplicate.";
        }

        public  List<CustomerDetails> GetAllApplicantDetails()
        {
            return _customerRepository.AsQueryable().ToList();
        }

        /// <summary>
        /// Map the customer details
        /// </summary>
        /// <param name="customerInformation"></param>
        /// <returns></returns>
        private CustomerDetails MapCustomerDetails(CustomerInformationCommand customerInformation)
        {
            return new CustomerDetails()
            {
                AccountId = Convert.ToInt16(customerInformation.AccountId),
                FirstName = customerInformation.FirstName,
                LastName = customerInformation.LastName
            };
        }

        /// <summary>
        /// Return false if record is duplicate
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private bool IsRecordAlredayExists(string accountId)
        {
           return (!string.IsNullOrWhiteSpace(accountId) && (GetAllApplicantDetails()
                .Where(x => x.AccountId == Convert.ToInt32(accountId)).Count() <= 0));
          
        }

    }



}
