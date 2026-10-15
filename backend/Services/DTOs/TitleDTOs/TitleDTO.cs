using services.DTOs.AuthorDTOs;
using services.DTOs.PublisherDTOs;

namespace services.DTOs.TitleDTOs;

public class TitleDTO
{
    public int TitleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly DateOfPublishing { get; set; }

    public IList<string> Genres { get; set; } = []; // FORMAT: genreA/genreB/.../genreX
    // Title number?

    public IList<AuthorLookupDTO> Authors { get; set; } = [];
    public PublisherLookupDTO? Publisher { get; set; }
}