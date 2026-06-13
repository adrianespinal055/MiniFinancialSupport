using FluentValidation;
using FluentValidation.Validators;
using MiniFinancialSupport.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniFinancialSupport.Application.Validators
{
    public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required")
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email format is invalid");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("DocumentNumber is required");

            RuleFor(x => x.Phone)
                .MaximumLength(20);

        }
    }
}
