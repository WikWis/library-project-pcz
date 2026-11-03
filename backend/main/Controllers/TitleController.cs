using Microsoft.AspNetCore.Mvc;
using Services.DTOs.TitleDTOs;
using Services.Interfaces;

namespace main.Controllers;

[ApiController]
[Route("/api/titles")]
public class TitleController : ControllerBase
{
    private readonly ITitleService _titleService;

    public TitleController(ITitleService titleService)
    {
        _titleService = titleService;
    }

    // GET /api/titles
    // Zwraca:
    // - 200 OK: lista IList<TitleLookupDTO>
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? authorId, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        try
        {
            var titleLookupDtos = authorId == null
                ? await _titleService.GetTitlesAsync(page, pageSize)
                : await _titleService.GetTitlesByAuthorAsync(page, pageSize, (int)authorId);
            return Ok(titleLookupDtos);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET /api/titles/{titleId}
    // Zwraca:
    // - 200 OK: obiekt TitleDTO
    // - 404 NotFound: brak obiektu (gdy ID nie istnieje w bazie)
    [HttpGet("{titleId:int}")]
    public async Task<IActionResult> Details(int titleId)
    {
        try 
        { 
            var titleDto = await _titleService.GetTitleAsync(titleId);
            return Ok(titleDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST /api/titles
    // Zwraca:
    // - 201 Created: obiekt TitleDTO
    // - 400 BadRequest: brak obiektu (gdy createTitleDto to null)
    // - 404 NotFound: brak wydawcy lub autora w bazie
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTitleDTO? createTitleDto)
    {
        if (createTitleDto == null)
            return BadRequest(new { message = "Brak danych w treści żądania." });

        try
        {
            int newTitleId = await _titleService.AddTitleAsync(createTitleDto);
            var createdTitle = await _titleService.GetTitleAsync(newTitleId);

            return CreatedAtAction(nameof(Details), new { titleId = newTitleId }, createdTitle);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT /api/titles/{titleId}
    // Zwraca:
    // - 200 OK: zaktualizowany obiekt TitleDTO
    // - 400 BadRequest: brak obiektu
    // - 404 NotFound: brak obiektu (gdy tytuł o danym titleId nie istnieje)
    [HttpPut("{titleId:int}")]
    public async Task<IActionResult> Update(int titleId, [FromBody] CreateTitleDTO? createTitleDto)
    {
        if (createTitleDto == null)
        {
            return BadRequest(new { message = "Brak danych w treści żądania." });
        }
        
        var updateDto = new UpdateTitleDTO()
        {
            TitleId = titleId,
            Name = createTitleDto.Name,
            DateOfPublishing = createTitleDto.DateOfPublishing,
            ISBN = createTitleDto.ISBN,
            Genres = createTitleDto.Genres,
            Authors = createTitleDto.Authors,
            Publisher = createTitleDto.Publisher
        };

        try
        {
            await _titleService.UpdateTitleAsync(updateDto);
            var updatedTitle = await _titleService.GetTitleAsync(titleId);

            return Ok(updatedTitle);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE /api/titles/{titleId}
    // Zwraca:
    // - 204 NoContent: brak obiektu (rekord został pomyślnie usunięty)
    // - 400 BadRequest: brak obiektu (gdy titleId to null)
    // - 404 NotFound: brak obiektu (gdy rekord o danym ID nie istnieje)
    [HttpDelete("{titleId:int}")]
    public async Task<IActionResult> Delete(int titleId)
    {
        try
        {
            await _titleService.DeleteTitleAsync(titleId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}