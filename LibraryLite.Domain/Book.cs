namespace LibraryLite.Domain;

public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = default!;
    public string Isbn { get; private set; } = default!;
    public int PublishedYear { get; private set; }
    public Guid AuthorId { get; private set; }

    // Constructor principal
    public Book(Guid id, string title, string isbn, int publishedYear, Guid authorId)
    {
        // Validaciones mínimas de dominio
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN cannot be empty.", nameof(isbn));

        Id = id;
        Title = title;
        Isbn = isbn;
        PublishedYear = publishedYear;
        AuthorId = authorId;
    }

    // Método para actualizar propiedades (en lugar de setters públicos)
    public void Update(string title, string isbn, int publishedYear, Guid authorId)
    {
        // Validaciones mínimas de dominio
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN cannot be empty.", nameof(isbn));

        Title = title;
        Isbn = isbn;
        PublishedYear = publishedYear;
        AuthorId = authorId;
    }
}
