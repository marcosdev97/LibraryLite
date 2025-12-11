namespace LibraryLite.Application.Authors;

public interface IAuthorService
{
    Task<IReadOnlyList<AuthorResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AuthorResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AuthorResponse> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken = default);
    Task<AuthorResponse?> UpdateAsync(Guid id, UpdateAuthorRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
