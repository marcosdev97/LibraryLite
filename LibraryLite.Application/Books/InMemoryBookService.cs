using LibraryLite.Application.Common;
using LibraryLite.Domain;

namespace LibraryLite.Application.Books;

/// Implementación simple en memoria para gestionar libros.
/// En un proyecto real esto hablaría con una base de datos.
public sealed class InMemoryBookService : IBookService
{
    // Colección en memoria simulando una tabla
    private readonly List<Book> _books = new();

    public InMemoryBookService()
    {
        // Semilla opcional de datos para probar paginación/filtrado
        var authorId = Guid.NewGuid();

        _books.Add(new Book(Guid.NewGuid(), "The Final Empire", "ISBN-001", 2006, authorId));
        _books.Add(new Book(Guid.NewGuid(), "The Well of Ascension", "ISBN-002", 2007, authorId));
        _books.Add(new Book(Guid.NewGuid(), "The Hero of Ages", "ISBN-003", 2008, authorId));
    }

    public Task<PagedResult<BookResponse>> GetAllAsync(
        string? search = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        // Aseguramos que page y pageSize tengan valores razonables
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // límite de seguridad

        // Query base
        IEnumerable<Book> query = _books;

        // Filtrado por texto (título o ISBN)
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower().Trim();
            query = query.Where(b =>
                b.Title.ToLower().Contains(search) ||
                b.Isbn.ToLower().Contains(search));
        }

        var totalCount = query.Count();

        // Paginación
        var items = query
            .OrderBy(b => b.Title) // orden básico por título
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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

        var result = new PagedResult<BookResponse>(items, totalCount, page, pageSize);

        return Task.FromResult(result);
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
