namespace services.DTOs.TitleDTOs;

public class TitleLookupDTO
{
    public int TitleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly DateOfPublishing { get; set; }

    public IList<string> Genres { get; set; } = []; // FORMAT: genreA/genreB/.../genreX
    // Title number?
}