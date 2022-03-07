using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Account.DomainApi.Common;
using Services.Account.DomainApi.Model;
using Services.Account.Repository.DomainRepository;
using Services.Account.Surface;

namespace Services.Account.DomainApi.Domain.Account.Processor
{
    public class AccountProcessor : IAccountProcessor
    {
        /// <summary>
        /// Logger object for applicantprocessor
        /// </summary>
        private ILogger<AccountProcessor> _logger { get; set; }

        /// <summary>
        /// Repository instance
        /// </summary>
        private readonly IMongoRepository<CustomerDetails> _customerRepository;

        /// <summary>
        /// Repository instance
        /// </summary>
        private readonly IMongoRepository<MeterDetails> _accountRepository;

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

        int duplicateRecord = 0, newRecords = 0, invalidMeterReading = 0, invalidAccount=0;

        public AccountProcessor(ILogger<AccountProcessor> logger, IOptions<AppSettings> appSettings, IWebHostEnvironment hostingEnv, IFileSystem fileSystem, IMongoRepository<CustomerDetails> customerRepository, IMongoRepository<MeterDetails> accountRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _hostingEnv = hostingEnv;
            _fileSystem = fileSystem;
            _appSettings = appSettings.Value;
        }

        public List<MeterDetails> GetAllMeterReading()
        {
           return _accountRepository.AsQueryable().ToList();
        }

        public async Task<string> ImportMeterReading()
        {
            List<MeterDetails> meterDetails = new List<MeterDetails>();
       

            var sourceFilePath = _hostingEnv.ContentRootPath + _fileSystem.Path.DirectorySeparatorChar + _appSettings.MeterReadingFilePath;

            if (_fileSystem.File.Exists(sourceFilePath))
            {
                // var productDataJsonString = _fileSystem.File.ReadAllText(sourceFilePath);
                var result = from line in File.ReadAllLines(sourceFilePath).Skip(1)
                             let columns = line.Split(',')
                             select new
                             {
                                 AccountId = columns[0],
                                 MeterReadingDateTime = columns[1],
                                 MeterReadValue = columns[2].TrimStart('0')
                             };

                foreach (var data in result)
                {
                    bool duplicateMeterReading = IsMeterReadingDuplicate(data.AccountId,data.MeterReadValue);
                    if (!duplicateMeterReading)
                    {
                        IsMeterReadingDuplicate(data.AccountId, data.MeterReadValue);
                        ++duplicateRecord;                  
                    }
                   else if(IsMeterAccountExists(data.AccountId))
                    { 
                        ++invalidAccount;
                    }
                    else if(IsMeterReadingValid(data.MeterReadValue))
                    {
                        meterDetails.Add(new MeterDetails()
                        {
                            AccountId = Convert.ToInt16(data.AccountId),
                            MeterReadingDate = data.MeterReadingDateTime,
                            MeterReadValue = data.MeterReadValue.TrimStart('0')
                        });

                         ++newRecords;
                    }
                }
                if (meterDetails.Count() > 0)
                    await _accountRepository.InsertManyAsync(meterDetails);

            }
            return $"{newRecords} has been added , {duplicateRecord} records are duplicate,{invalidAccount} meterreading belongs to invalid account and {invalidMeterReading} invalid reading format";
        }

        #region Helper method

        /// <summary>
        /// Checks if meter reading in valid format
        /// </summary>
        /// <param name="meterReading"></param>
        /// <returns></returns>
        private bool IsMeterReadingValid(string meterReading)
        {
            if (!Regex.IsMatch(meterReading, RegexConstants.ReadingFormat))
            {
               ++invalidMeterReading;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if meter reading already exist in collection
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="meterReading"></param>
        /// <returns></returns>
        private bool IsMeterReadingDuplicate(string accountId , string meterReading)
        {
            return (GetAllMeterReading()
               .Where(x => x.AccountId == Convert.ToInt32(accountId) && x.MeterReadValue == meterReading).Count() <= 0);
        }

        /// <summary>
        /// Checks if accountId exist in customerdetails collection
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private bool IsMeterAccountExists(string accountId)
        {
            return (_customerRepository.AsQueryable()
                .Where(x => x.AccountId == Convert.ToInt32(accountId)).Count() <= 0);
        }

        #endregion

    }
}
