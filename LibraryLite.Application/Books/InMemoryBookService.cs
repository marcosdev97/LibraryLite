using LibraryLite.Domain;

namespace LibraryLite.Application.Books;

/// Implementación simple en memoria para gestionar libros.
/// En un proyecto real esto hablaría con una base de datos.
public sealed class InMemoryBookService : IBookService
{
    // Colección en memoria simulando una tabla
    private readonly List<Book> _books = new();

    public Task<IReadOnlyList<BookResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Mapear entidades de dominio a DTOs de respuesta
        var result = _books
            .Select(b => new BookResponse
            {
                Id = b.Id,
                Title = b.Title,
                Isbn = b.Isbn,
                PublishedYear = b.PublishedYear,
                AuthorId = b.AuthorId
            })
            .ToList()
            .AsReadOnly();

        return Task.FromResult((IReadOnlyList<BookResponse>)result);
    }

    public Task<BookResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book is null)
            return Task.FromResult<BookResponse?>(null);

        var dto = new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            PublishedYear = book.PublishedYear,
            AuthorId = book.AuthorId
        };

        return Task.FromResult<BookResponse?>(dto);
    }

    public Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken = default)
    {
        var book = new Book(
            id: Guid.NewGuid(),
            title: request.Title,
            isbn: request.Isbn,
            publishedYear: request.PublishedYear,
            authorId: request.AuthorId);

        _books.Add(book);

        var dto = new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            PublishedYear = book.PublishedYear,
            AuthorId = book.AuthorId
        };

        return Task.FromResult(dto);
    }

    public async Task<BookResponse?> UpdateAsync(Guid id, UpdateBookRequest request, CancellationToken cancellationToken = default)
    {
        var existing = _books.FirstOrDefault(b => b.Id == id);
        if (existing is null)
            return null;

        existing.Update(
            title: request.Title,
            isbn: request.Isbn,
            publishedYear: request.PublishedYear,
            authorId: request.AuthorId);

        return await GetByIdAsync(id, cancellationToken);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = _books.FirstOrDefault(b => b.Id == id);
        if (existing is null)
            return Task.FromResult(false);

        _books.Remove(existing);
        return Task.FromResult(true);
    }
}
