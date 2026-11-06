using Microsoft.AspNetCore.Mvc;
using services.DTOs.BorrowingDTOs;
using Services.Interfaces;

namespace main.Controllers;

[ApiController]
[Route("/api/borrowings")]
public class BorrowingController : Controller
{
    private readonly IBorrowingService _borrowingService;
    public BorrowingController(IBorrowingService borrowingService)
    {
        _borrowingService = borrowingService;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] int page = 0, [FromQuery] int pageSize = 20, [FromQuery] string? userId = null)
    {
        if(userId == null)
            return BadRequest();
        var borrowings = _borrowingService.GetBorrowingsByUser(page, pageSize, userId);
        return Ok(borrowings);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] CreateBorrowingDTO? createBorrowingDto)
    {
        if (createBorrowingDto == null) return BadRequest();
        try
        {
            var borrowingDto = _borrowingService.AddBorrowing(createBorrowingDto);
            return Created("/api/borrowings/" + borrowingDto?.BorrowingId, borrowingDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
    [HttpPut("{borrowingId:int?}")]
    public IActionResult Update(int? borrowingId, [FromBody] UpdateBorrowingDTO? updateBorrowingDto)
    {
        if (borrowingId == null || updateBorrowingDto == null) return BadRequest();
        try
        {
            updateBorrowingDto.BorrowingId = borrowingId;
            var borrowingDto = _borrowingService.UpdateBorrowing(updateBorrowingDto);
            return borrowingDto == null ? NotFound() : Ok(borrowingDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
    [HttpDelete("{borrowingId:int?}")]
    public IActionResult Delete(int? borrowingId)
    {
        if (borrowingId == null) return BadRequest();
        try
        {
            var result = _borrowingService.DeleteBorrowing((int)borrowingId);
            return result ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}