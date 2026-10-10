using services.DTOs.TitleDTOs;

namespace services.DTOs.PublisherDTOs;

public class PublisherDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<TitleLookupDTO> Titles { get; set; } = [];
}