using System;
using FluentValidation;
using Services.Account.DomainApi.Common;

namespace Services.Account.DomainApi.ViewModel
{
    public class CustomerInformationCommand
    {
        /// <summary>
        /// Unique identifier of the account
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Users last name
        /// </summary>
        public string LastName { get; set; }
    }

    public class CustomerInformationValidator : AbstractValidator<CustomerInformationCommand>
    {
        /// <summary>
        /// Constructor
        /// </summary>   
        public CustomerInformationValidator()
        {
            RuleFor(x => x.AccountId)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(ValidationMessages.EmptyCheck)
               .NotNull().WithMessage(ValidationMessages.NullCheck)
               .Matches(RegexConstants.AccountNumber).WithMessage(ValidationMessages.InvalidFormat);

            RuleFor(x => x.FirstName)
              .Cascade(CascadeMode.Stop)
              .NotEmpty().WithMessage(ValidationMessages.EmptyCheck)
              .NotNull().WithMessage(ValidationMessages.NullCheck)
              .Length(1, 15).WithMessage(ValidationMessages.InvalidLength)
              .Matches(RegexConstants.NameFormat).WithMessage(ValidationMessages.InvalidFormat);

            RuleFor(x => x.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ValidationMessages.EmptyCheck)
            .NotNull().WithMessage(ValidationMessages.NullCheck)
            .Length(1, 30).WithMessage(ValidationMessages.InvalidLength)
            .Matches(RegexConstants.NameFormat).WithMessage(ValidationMessages.InvalidFormat);

        }
    }
}

