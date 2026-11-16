using Model;

namespace services.DTOs.BorrowingDTOs;

public class ReserveBorrowingDTO
{
    public DateOnly StartDate { get; set; }
    public int PieceId { get; set; }
    public string? UserId { get; set; }
}