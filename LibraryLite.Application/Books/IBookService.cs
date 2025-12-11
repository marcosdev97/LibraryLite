using LibraryLite.Domain;

namespace LibraryLite.Application.Books;

public interface IBookService
{
    Task<IReadOnlyList<BookResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BookResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken = default);
    Task<BookResponse?> UpdateAsync(Guid id, UpdateBookRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
