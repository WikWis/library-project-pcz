using Microsoft.AspNetCore.Mvc;
using services.DTOs.AuthorDTOs;
using Services.Interfaces;

namespace main.Controllers;

[ApiController]
[Route("/api/authors")]
public class AuthorController : Controller
{
    private readonly IAuthorService authorService;

    public AuthorController(IAuthorService authorService)
    {
        this.authorService = authorService;
    }

    /// <summary>
    /// Get information about authors. Uses pagination.
    /// </summary>
    /// <param name="titleId">Query parameter containing title identifier used in database.</param>
    /// <param name="page">Query parameter containing chosen page number. Defaults to 0.</param>
    /// <param name="pageSize">Query parameter containing page size. Defaults to 20.</param>
    /// <returns>
    /// Unless titleId is provided, returns list of all authors (200 OK),
    /// otherwise returns list of authors associated with chosen title (200 OK).
    /// </returns>
    [HttpGet]
    public IActionResult Index([FromQuery] int? titleId, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        var authorLookupDtos = titleId == null
            ? authorService.GetAuthors(page, pageSize)
            : authorService.GetAuthorsByTitle(page, pageSize, (int)titleId);
        return Ok(authorLookupDtos);
    }

    /// <summary>
    /// Get detailed information about single author
    /// </summary>
    /// <param name="authorId">Author identifier used in database</param>
    /// <returns>
    /// If author exists, returns information about author (200 OK),
    /// otherwise returns "not found" (404 Not Found).
    /// </returns>
    [HttpGet("{authorId:int}")]
    public IActionResult Details(int authorId)
    {
        var authorDto =  authorService.GetAuthor(authorId);
        return authorDto == null ? NotFound() : Ok(authorDto);
    }
    
    /// <summary>
    /// Create new entry of author in database.
    /// </summary>
    /// <param name="createAuthorDto">New author data.</param>
    /// <returns>
    /// If createAuthorDto is correct, returns "created" (201 Created) with URL to new resource (Location response header) and newly created resource itself (JSON in response body),
    /// otherwise returns "bad request" (400 Bad Request).
    /// </returns>
    [HttpPost]
    public IActionResult Create([FromBody] CreateAuthorDTO? createAuthorDto)
    {
        if (createAuthorDto == null) return BadRequest();
        var id = authorService.AddAuthor(createAuthorDto);
        var author = authorService.GetAuthor(id);
        return Created("/api/authors/" + id, author);
    }

    /// <summary>
    /// Update information about chosen author.
    /// </summary>
    /// <param name="authorId">Author identifier used in database.</param>
    /// <param name="updateAuthorDto">Modified author data.</param>
    /// <returns>
    /// If authorId is missing or updateAuthorDto is incorrect, returns "bad request" (400 Bad Request),
    /// otherwise if succeeds, returns updated author information (200 OK),
    /// otherwise returns "not found" (404 Not Found).
    /// </returns>
    [HttpPut("{authorId:int?}")]
    public IActionResult Update(int? authorId, [FromBody] CreateAuthorDTO? updateAuthorDto)
    {
        if (authorId == null || updateAuthorDto == null) return BadRequest();
        var updateDto = new UpdateAuthorDTO()
        {
            AuthorId = (int)authorId,
            Name = updateAuthorDto.Name,
            Surname = updateAuthorDto.Surname
        };
        var result = authorService.UpdateAuthor(updateDto); 
        return result ? Ok(authorService.GetAuthor((int)authorId)) : NotFound();
    }
    
    /// <summary>
    /// Delete entry of chosen author.
    /// </summary>
    /// <param name="authorId">Author identifier used in database.</param>
    /// <returns>
    /// If authorId is missing, returns "bad request" (400 Bad Request),
    /// otherwise if operation is currently invalid, returns "conflict" (409 Conflict),
    /// otherwise if succeeds, returns "no content" (204 NoContent),
    /// otherwise returns "not found" (404 Not Found).
    /// </returns>
    [HttpDelete("{authorId:int?}")]
    public IActionResult Delete(int? authorId)
    {
        if (authorId == null) return BadRequest();
        try
        {
            var result = authorService.DeleteAuthor((int)authorId);
            return result ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}