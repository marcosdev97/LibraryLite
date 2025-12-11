using FluentValidation;
using LibraryLite.Application.Books;

public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
{
    public UpdateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Isbn)
            .NotEmpty()
            .MinimumLength(5);

        RuleFor(x => x.PublishedYear)
            .InclusiveBetween(1500, DateTime.UtcNow.Year);

        RuleFor(x => x.AuthorId)
            .NotEmpty();
    }
}
