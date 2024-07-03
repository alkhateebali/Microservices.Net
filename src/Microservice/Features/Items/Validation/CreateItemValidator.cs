using FluentValidation;
using Microservice.Features.Items.Commands;

namespace Microservice.Features.Items.Validation;

public class CreateItemValidator:AbstractValidator<CreateItem.Command>
{
    public CreateItemValidator()
    { 
        RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify a name");
        // Add other validation rules as necessary
    }
}