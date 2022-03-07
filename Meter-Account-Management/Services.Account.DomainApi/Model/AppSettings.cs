using System;
namespace Services.Account.DomainApi.Model
{
    /// <summary>
    /// Configuration Settings fo meter account
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// File path of meterReadingFilePath
        /// </summary>
        public string MeterReadingFilePath { get; set; }


        /// <summary>
        /// File path of testAccountFilePath
        /// </summary>
        public string TestAccountFilePath { get; set; }
    }
}
