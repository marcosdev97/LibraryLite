using LibraryLite.Application.Common;

namespace LibraryLite.Application.Books;

public interface IBookService
{
    Task<PagedResult<BookResponse>> GetAllAsync(
        string? search = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<BookResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken = default);
    Task<BookResponse?> UpdateAsync(Guid id, UpdateBookRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
