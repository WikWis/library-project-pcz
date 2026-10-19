using Microsoft.AspNetCore.Mvc;
using Services.DTOs.PieceDTOs;
using Services.Interfaces;

namespace main.Controllers
{
    public class PieceController : Controller
    {
        private readonly IPieceService pieceService;

        public PieceController(IPieceService pieceService)
        {
            this.pieceService = pieceService;
        }

        // GET /api/pieces
        // Zwraca:
        // - 200 OK: lista IList<PieceLookupDTO>
        [HttpGet]
        public IActionResult Index([FromQuery] int? titleId, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
        {
            var pieceLookupDtos = titleId == null
                ? pieceService.GetPieces(page, pageSize)
                : pieceService.GetPiecesByTitle(page, pageSize, (int)titleId);

            return Ok(pieceLookupDtos);
        }

        // GET /api/pieces/{pieceId}
        // Zwraca:
        // - 200 OK: obiekt PieceDTO
        // - 404 NotFound: brak obiektu (gdy ID nie istnieje w bazie)
        [HttpGet("{pieceId:int}")]
        public IActionResult Details(int pieceId)
        {
            var pieceDto = pieceService.GetPiece(pieceId);
            return pieceDto == null ? NotFound() : Ok(pieceDto);
        }

        // POST /api/pieces?titleId=1
        // Zwraca:
        // - 201 Created: obiekt PieceDTO (oraz nagłówek Location: /api/pieces/{id})
        // - 400 BadRequest: brak obiektu lub brak poprawnego titleId
        // - 404 NotFound: brak wskazanego tytułu (gdy serwis zwróci 0)
        [HttpPost]
        public IActionResult Create([FromQuery] int? titleId, [FromBody] CreatePieceDTO? createPieceDto)
        {
            if (createPieceDto == null || titleId == null) return BadRequest();

            var id = pieceService.AddPiece((int)titleId, createPieceDto);
            if (id == 0) return NotFound();

            var piece = pieceService.GetPiece(id);
            return Created("/api/pieces/" + id, piece);
        }

        // PUT /api/pieces/{pieceId}
        // Zwraca:
        // - 200 OK: zaktualizowany obiekt PieceDTO
        // - 400 BadRequest: brak obiektu (gdy pieceId to null lub createPieceDto to null)
        // - 404 NotFound: brak obiektu (gdy egzemplarz o danym pieceId nie istnieje)
        [HttpPut("{pieceId:int?}")]
        public IActionResult Update(int? pieceId, [FromBody] CreatePieceDTO? createPieceDto)
        {
            if (pieceId == null || createPieceDto == null) return BadRequest();

            var updateDto = new UpdatePieceDTO
            {
                PieceId = (int)pieceId,
                Status = createPieceDto.Status,
                State = createPieceDto.State
            };

            var result = pieceService.UpdatePiece(updateDto);
            return result ? Ok(pieceService.GetPiece((int)pieceId)) : NotFound();
        }

        // DELETE /api/pieces/{pieceId}
        // Zwraca:
        // - 204 NoContent: rekord pomyślnie usunięty
        // - 400 BadRequest: brak obiektu (gdy pieceId to null)
        // - 404 NotFound: rekord o danym ID nie istnieje
        [HttpDelete("{pieceId:int?}")]
        public IActionResult Delete(int? pieceId)
        {
            if (pieceId == null) return BadRequest();
            var result = pieceService.DeletePiece((int)pieceId);

            return result ? NoContent() : NotFound();
        }
    }
}
