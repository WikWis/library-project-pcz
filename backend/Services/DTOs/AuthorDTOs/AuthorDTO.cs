using services.DTOs.TitleDTOs;

namespace services.DTOs.AuthorDTOs;

public class AuthorDTO
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public IList<TitleLookupDTO> Titles { get; set; } = [];
}