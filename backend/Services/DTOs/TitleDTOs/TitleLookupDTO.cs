namespace Services.DTOs.TitleDTOs;

public class TitleLookupDTO
{
    public int TitleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly DateOfPublishing { get; set; }
    public long ISBN { get; set; }

    public IList<string> Genres { get; set; } = []; // FORMAT: genreA/genreB/.../genreX
}