namespace LibraryLite.Application.Books;

// DTO para crear un libro
public sealed class CreateBookRequest
{
    public string Title { get; set; } = default!;
    public string Isbn { get; set; } = default!;
    public int PublishedYear { get; set; }
    public Guid AuthorId { get; set; }
}

// DTO para actualizar un libro
public sealed class UpdateBookRequest
{
    public string Title { get; set; } = default!;
    public string Isbn { get; set; } = default!;
    public int PublishedYear { get; set; }
    public Guid AuthorId { get; set; }
}

// DTO de respuesta
public sealed class BookResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Isbn { get; set; } = default!;
    public int PublishedYear { get; set; }
    public Guid AuthorId { get; set; }
}
