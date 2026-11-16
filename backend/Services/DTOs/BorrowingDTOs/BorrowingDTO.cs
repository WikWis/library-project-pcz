using Model;
using Services.DTOs.PieceDTOs;

namespace services.DTOs.BorrowingDTOs;

public class BorrowingDTO
{
    public int BorrowingId { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public BorrowingStatus Status { get; set; }

    public PieceLookupDTO Piece { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
}