using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using services.DTOs.BorrowingDTOs;
using Services.Interfaces;

namespace main.Controllers;

[ApiController]
[Route("/api/borrowings")]
public class BorrowingController : Controller
{
    private readonly IBorrowingService borrowingService;

    public BorrowingController(IBorrowingService borrowingService)
    {
        this.borrowingService = borrowingService;
    }
    [HttpGet]
    public IActionResult Index([FromQuery] BorrowingStatus? status, [FromQuery] string? userId, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        IList<BorrowingDTO> borrowings;
        if (status != null)
            borrowings = borrowingService.GetBorrowingsByStatus(page, pageSize, status.Value);
        else if (userId != null)
            borrowings = borrowingService.GetBorrowingsByUser(page, pageSize, userId);
        else return BadRequest();
        
        return Ok(borrowings);
    }

    [HttpPost("reserve")]
    [Authorize(Roles = "reader")]
    public IActionResult Reserve(CreateBorrowingDTO? createBorrowingDto)
    {
        if(createBorrowingDto == null) return BadRequest();
        createBorrowingDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            var borrowing = borrowingService.ReserveBorrowing(createBorrowingDto);
            return Ok(borrowing);
        }
        catch(InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpDelete("cancel/{borrowingId:int}")]
    [Authorize(Roles = "reader")]
    public IActionResult Cancel(int? borrowingId)
    {
        if (borrowingId == null) return BadRequest();
        try
        {
            bool result = borrowingService.CancelBorrowing(borrowingId.Value,
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );
            return result ? NoContent() : NotFound();
        }
        catch(InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPatch("confirm/{borrowingId:int}")]
    [Authorize(Roles = "librarian")]
    public IActionResult Confirm(int? borrowingId)
    {
        if(borrowingId == null) return BadRequest();
        try
        {
            var borrowing = borrowingService.ConfirmBorrowing(borrowingId.Value);
            return Ok(borrowing);
        }
        catch(InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    
    [HttpPost("borrow")]
    [Authorize(Roles = "librarian")]
    public IActionResult Borrow(CreateBorrowingDTO? createBorrowingDto)
    {
        if(createBorrowingDto == null) return BadRequest();
        try
        {
            var borrowing = borrowingService.CreateBorrowing(createBorrowingDto);
            return Ok(borrowing);
        }
        catch(InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
    }
    
    [HttpPatch("return/{borrowingId:int}")]
    [Authorize(Roles = "librarian")]
    public IActionResult Return(int? borrowingId)
    {
        if(borrowingId == null) return BadRequest();
        try
        {
            var borrowing = borrowingService.ReturnBorrowing(borrowingId.Value);
            return Ok(borrowing);
        }
        catch(InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpDelete("{borrowingId:int}")]
    [Authorize(Roles = "librarian")]
    public IActionResult Delete(int? borrowingId)
    {
        if(borrowingId == null) return BadRequest();
        try
        {
            bool result = borrowingService.DeleteBorrowing(borrowingId.Value);
            return result ? NoContent() : NotFound();
        }
        catch(InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
    }
}