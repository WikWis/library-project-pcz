using services.DTOs.AuthorDTOs;

namespace Services.Interfaces;

public interface IAuthorService
{
    AuthorDTO? GetAuthor(int authorId);
    IList<AuthorLookupDTO> GetAuthors(int page, int pageSize);
    IList<AuthorLookupDTO> GetAuthorsByTitle(int page, int pageSize, int titleId);
    
    int AddAuthor(CreateAuthorDTO createAuthorDto);
    bool UpdateAuthor(UpdateAuthorDTO updateAuthorDto);
    bool DeleteAuthor(int authorId);
}