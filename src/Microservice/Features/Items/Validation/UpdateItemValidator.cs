using FluentValidation;
using Microservice.Features.Items.Commands;

namespace Microservice.Features.Items.Validation;

public class UpdateItemValidator : AbstractValidator<UpdateItem.Command>
{
    public UpdateItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify a  name");
        // Add other validation rules as necessary
    }

}