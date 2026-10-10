namespace model;

public class Publisher
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Title> Titles { get; set; } = [];
}