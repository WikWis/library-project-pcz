using Microsoft.AspNetCore.Mvc;
using Services.DTOs.TitleDTOs;
using Services.Interfaces;

namespace main.Controllers;

[ApiController]
[Route("/api/titles")]
public class TitleController : Controller
{
    private readonly ITitleService titleService;

    public TitleController(ITitleService titleService)
    {
        this.titleService = titleService;
    }

    // GET /api/titles
    // Zwraca:
    // - 200 OK: lista IList<TitleLookupDTO>
    [HttpGet]
    public IActionResult Index([FromQuery] int? authorId, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        var titleLookupDtos = authorId == null
            ? titleService.GetTitles(page, pageSize)
            : titleService.GetTitlesByAuthor(page, pageSize, (int)authorId);
        return Ok(titleLookupDtos);
    }

    // GET /api/titles/{titleId}
    // Zwraca:
    // - 200 OK: obiekt TitleDTO
    // - 404 NotFound: brak obiektu (gdy ID nie istnieje w bazie)
    [HttpGet("{titleId:int}")]
    public IActionResult Details(int titleId)
    {
        var titleDto = titleService.GetTitle(titleId);
        return titleDto == null ? NotFound() : Ok(titleDto);
    }

    // POST /api/titles
    // Zwraca:
    // - 201 Created: obiekt TitleDTO (oraz nagłówek Location: /api/titles/{id})
    // - 400 BadRequest: brak obiektu (gdy createTitleDto to null)
    [HttpPost]
    public IActionResult Create([FromBody] CreateTitleDTO? createTitleDto)
    {
        if (createTitleDto == null) return BadRequest();

        var id = titleService.AddTitle(createTitleDto);
        var title = titleService.GetTitle(id);

        return Created("/api/titles/" + id, title);
    }

    // PUT /api/titles/{titleId}
    // Zwraca:
    // - 200 OK: zaktualizowany obiekt TitleDTO
    // - 400 BadRequest: brak obiektu (gdy titleId to null lub createTitleDto to null)
    // - 404 NotFound: brak obiektu (gdy tytuł o danym titleId nie istnieje)
    [HttpPut("{titleId:int?}")]
    public IActionResult Update(int? titleId, [FromBody] CreateTitleDTO? createTitleDto)
    {
        if (titleId == null || createTitleDto == null) return BadRequest();
        var updateDto = new UpdateTitleDTO()
        {
            TitleId = (int)titleId,
            Name = createTitleDto.Name,
            DateOfPublishing = createTitleDto.DateOfPublishing,
            ISBN = createTitleDto.ISBN,
            Genres = createTitleDto.Genres,
            Authors = createTitleDto.Authors,
            Publisher = createTitleDto.Publisher
        };

        var result = titleService.UpdateTitle(updateDto);
        return result ? Ok(titleService.GetTitle((int)titleId)) : NotFound();
    }

    // DELETE /api/titles/{titleId}
    // Zwraca:
    // - 204 NoContent: brak obiektu (rekord został pomyślnie usunięty)
    // - 400 BadRequest: brak obiektu (gdy titleId to null)
    // - 404 NotFound: brak obiektu (gdy rekord o danym ID nie istnieje)
    [HttpDelete("{titleId:int?}")]
    public IActionResult Delete(int? titleId)
    {
        if (titleId == null) return BadRequest();
        var result = titleService.DeleteTitle((int)titleId);

        return result ? NoContent() : NotFound();
    }
}