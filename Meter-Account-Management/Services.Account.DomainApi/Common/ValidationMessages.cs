using System;
namespace Services.Account.DomainApi.Common
{
    public struct ValidationMessages
    {
        public const string NullCheck = "{PropertyName} is null";

        public const string EmptyCheck = "{PropertyName} is Empty";

        public const string InvalidFormat = "Invalid {PropertyName}";

        public const string InvalidLength = "{PropertyName} has invalid lenght";
    }

}
