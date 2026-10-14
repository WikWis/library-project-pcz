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
        var authors = titleId == null
            ? authorService.GetAuthors(page, pageSize)
            : authorService.GetAuthorsByTitle(page, pageSize, (int)titleId);
        return Ok(authors);
    }

    [HttpGet("{authorId:int}")]
    public IActionResult Details(int authorId)
    {
        var authorDto =  authorService.GetAuthor(authorId);
        return authorDto == null ? NotFound() : Ok(authorDto);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] CreateAuthorDTO? authorDto)
    {
        if (authorDto == null) return BadRequest();
        var id = authorService.AddAuthor(authorDto);
        var author = authorService.GetAuthor(id);
        return Created("/api/authors/" + id, author);
    }

    [HttpPut("{authorId:int?}")]
    public IActionResult Update(int? authorId, [FromBody] CreateAuthorDTO? authorDto)
    {
        if (authorId == null || authorDto == null) return BadRequest();
        var updateDto = new UpdateAuthorDTO()
        {
            AuthorId = (int)authorId,
            Name = authorDto.Name,
            Surname = authorDto.Surname
        };
        var result = authorService.UpdateAuthor(updateDto); 
        return result ? NoContent() : NotFound();
    }
    
    [HttpDelete("{authorId:int?}")]
    public IActionResult Update(int? authorId)
    {
        if (authorId == null) return BadRequest();
        var result = authorService.DeleteAuthor((int)authorId); 
        return result ? NoContent() : NotFound();
    }
}