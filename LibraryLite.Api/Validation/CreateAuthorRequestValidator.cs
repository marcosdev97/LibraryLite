using FluentValidation;
using LibraryLite.Application.Authors;

public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
{
    public CreateAuthorRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del autor es obligatorio.")
            .MaximumLength(100);
    }
}
