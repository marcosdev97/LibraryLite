namespace LibraryLite.Domain;

public class Author
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;

    public Author(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        Id = id;
        Name = name;
    }
}
