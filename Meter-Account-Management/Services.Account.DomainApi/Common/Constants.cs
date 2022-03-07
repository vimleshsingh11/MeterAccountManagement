using System;
namespace Services.Account.DomainApi.Common
{

    /// <summary>
    /// Regex
    /// </summary>
    public struct RegexConstants
    {
        /// <summary>
        /// check for 5 digits
        /// </summary>
        public const string AccountNumber = @"^[0-9]{5}$";

        /// <summary>
        /// check for 5 digits
        /// </summary>
        public const string ReadingFormat = @"^[0-9]{5}$";

        /// <summary>
        /// string  value
        /// </summary>
        public const string NameFormat = "^(?i)[a-zA-Z]+([a-zA-Z\\s\\-\\']*[a-zA-Z\\-\\'])?$";
    }
}
