namespace model;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public ICollection<Title> Titles { get; set; } = [];
}