using services.DTOs.AuthorDTOs;

namespace Services.Interfaces;

public interface IAuthorService
{
    AuthorDTO? GetAuthor(int id);
    IList<AuthorLookupDTO> GetAuthors(int page, int pageSize);
    IList<AuthorLookupDTO> GetAuthorsByTitle(int page, int pageSize, int titleId);
    
    int AddAuthor(CreateAuthorDTO dto);
    bool UpdateAuthor(UpdateAuthorDTO dto);
    bool DeleteAuthor(int id);
}