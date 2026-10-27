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

    [HttpGet]
    public IActionResult Index([FromQuery] int? titleId, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        var authorLookupDtos = titleId == null
            ? authorService.GetAuthors(page, pageSize)
            : authorService.GetAuthorsByTitle(page, pageSize, (int)titleId);
        return Ok(authorLookupDtos);
    }

    [HttpGet("{authorId:int}")]
    public IActionResult Details(int authorId)
    {
        var authorDto =  authorService.GetAuthor(authorId);
        return authorDto == null ? NotFound() : Ok(authorDto);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] CreateAuthorDTO? createAuthorDto)
    {
        if (createAuthorDto == null) return BadRequest();
        var id = authorService.AddAuthor(createAuthorDto);
        var author = authorService.GetAuthor(id);
        return Created("/api/authors/" + id, author);
    }

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