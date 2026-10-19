namespace services.DTOs.AuthorDTOs;

public class UpdateAuthorDTO
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
}