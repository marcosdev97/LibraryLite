namespace LibraryLite.Application.Authors;

public sealed class CreateAuthorRequest
{
    public string Name { get; set; } = default!;
}

public sealed class UpdateAuthorRequest
{
    public string Name { get; set; } = default!;
}

public sealed class AuthorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
