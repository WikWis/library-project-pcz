using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    /// Get information about borrowings of chosen user. Uses pagination.
    /// </summary>
    /// <param name="userId">Query parameter containing user identifier used in database.</param>
    /// <param name="page">Query parameter containing chosen page number. Defaults to 0.</param>
    /// <param name="pageSize">Query parameter containing page size. Defaults to 20.</param>
    /// <returns>
    /// Unless titleId is provided, returns list of all authors (200 OK),
    /// otherwise returns "bad request" (400 Bad Request).
    /// </returns>
    [HttpGet]
    public IActionResult Index([FromQuery] int page = 0, [FromQuery] int pageSize = 20, [FromQuery] string? userId = null)
    {
        if(userId == null)
            return BadRequest();
        var borrowings = _borrowingService.GetBorrowingsByUser(page, pageSize, userId);
        return Ok(borrowings);
    }
    
    /// <summary>
    /// Delete entry of chosen borrowing.
    /// </summary>
    /// <param name="borrowingId">Borrowing identifier used in database.</param>
    /// <returns>
    /// If borrowingId is missing, returns "bad request" (400 Bad Request),
    /// otherwise if operation is currently invalid, returns "conflict" (409 Conflict),
    /// otherwise if succeeds, returns "no content" (204 NoContent),
    /// otherwise returns "not found" (404 Not Found).
    /// </returns>
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