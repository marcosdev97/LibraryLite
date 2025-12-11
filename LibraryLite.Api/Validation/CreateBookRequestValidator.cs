using FluentValidation;
using LibraryLite.Application.Books;

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.")
            .MaximumLength(100);

        RuleFor(x => x.Isbn)
            .NotEmpty().WithMessage("El ISBN es obligatorio.")
            .MinimumLength(5).WithMessage("El ISBN debe tener al menos 5 caracteres.");

        RuleFor(x => x.PublishedYear)
            .InclusiveBetween(1500, DateTime.UtcNow.Year)
            .WithMessage("El año de publicación no es válido.");

        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("El autor es obligatorio.");
    }
}
