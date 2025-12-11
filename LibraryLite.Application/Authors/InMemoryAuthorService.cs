using LibraryLite.Domain;

namespace LibraryLite.Application.Authors;

/// Servicio en memoria para gestionar autores.
/// En un futuro podría sustituirse por un repositorio con base de datos.
public sealed class InMemoryAuthorService : IAuthorService
{
    private readonly List<Author> _authors = new();

    public InMemoryAuthorService()
    {
        // Semilla opcional de autores de prueba
        _authors.Add(new Author(Guid.NewGuid(), "Brandon Sanderson"));
        _authors.Add(new Author(Guid.NewGuid(), "J. K. Rowling"));
        _authors.Add(new Author(Guid.NewGuid(), "George R. R. Martin"));
    }

    public Task<IReadOnlyList<AuthorResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = _authors
            .Select(a => new AuthorResponse
            {
                Id = a.Id,
                Name = a.Name
            })
            .ToList()
            .AsReadOnly();

        return Task.FromResult((IReadOnlyList<AuthorResponse>)result);
    }

    public Task<AuthorResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);
        if (author is null)
            return Task.FromResult<AuthorResponse?>(null);

        var dto = new AuthorResponse
        {
            Id = author.Id,
            Name = author.Name
        };

        return Task.FromResult<AuthorResponse?>(dto);
    }

    public Task<AuthorResponse> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken = default)
    {
        var author = new Author(Guid.NewGuid(), request.Name);
        _authors.Add(author);

        var dto = new AuthorResponse
        {
            Id = author.Id,
            Name = author.Name
        };

        return Task.FromResult(dto);
    }

    public async Task<AuthorResponse?> UpdateAsync(Guid id, UpdateAuthorRequest request, CancellationToken cancellationToken = default)
    {
        var existing = _authors.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return null;

        // Como la entidad Author solo tiene Name, podemos recrearla o exponer un método Update.
        var updated = new Author(existing.Id, request.Name);

        _authors.Remove(existing);
        _authors.Add(updated);

        return await GetByIdAsync(id, cancellationToken);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = _authors.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return Task.FromResult(false);

        _authors.Remove(existing);
        return Task.FromResult(true);
    }
}
