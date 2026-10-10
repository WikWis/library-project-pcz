namespace model;

public class Title
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly DateOfPublishing { get; set; }

    public string Genres { get; set; } = string.Empty; // FORMAT: genreA/genreB/.../genreX
    // Title number?

    public int PublisherId { get; set; }
    public Publisher? Publisher { get; set; }
    public ICollection<Author> Authors { get; set; } = [];
}