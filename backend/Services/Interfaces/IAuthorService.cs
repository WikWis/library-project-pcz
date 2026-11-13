using services.DTOs.AuthorDTOs;

namespace Services.Interfaces;

public interface IAuthorService
{
    /// <summary>
    /// Get detailed information about single author
    /// </summary>
    /// <param name="authorId">Author identifier used in database</param>
    /// <returns>Information about author, including associated titles</returns>
    AuthorDTO? GetAuthor(int authorId);
    
    /// <summary>
    /// Get information about every author. Uses pagination.
    /// </summary>
    /// <param name="page">Chosen page number. Starts with 0.</param>
    /// <param name="pageSize">Page size.</param>
    /// <returns>Information about authors. Amount of entries is equal to pageSize.</returns>
    IList<AuthorLookupDTO> GetAuthors(int page, int pageSize);
    
    /// <summary>
    /// Get information about every author associated to chosen title. Uses pagination.
    /// </summary>
    /// <param name="page">Chosen page number. Starts with 0.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="titleId">Title identifier used in database.</param>
    /// <returns>Information about authors. Amount of entries is equal to pageSize.</returns>
    IList<AuthorLookupDTO> GetAuthorsByTitle(int page, int pageSize, int titleId);
    
    /// <summary>
    /// Create new entry of author in database.
    /// </summary>
    /// <param name="createAuthorDto">New authors data.</param>
    /// <returns>Identifier of newly created entry.</returns>
    int AddAuthor(CreateAuthorDTO createAuthorDto);
    
    /// <summary>
    /// Update information about chosen author.
    /// </summary>
    /// <param name="updateAuthorDto">Modified authors data.</param>
    /// <returns>Information if operation succeeded</returns>
    bool UpdateAuthor(UpdateAuthorDTO updateAuthorDto);

    /// <summary>
    /// Delete entry of chosen author.
    /// </summary>
    /// <param name="authorId">Author identifier used in database</param>>
    /// <returns>Information if operation succeeded</returns>
    bool DeleteAuthor(int authorId);
}