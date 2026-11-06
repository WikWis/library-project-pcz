namespace services.DTOs.BorrowingDTOs;

public class CreateBorrowingDTO
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public int PieceId { get; set; }
    public string UserId { get; set; } = string.Empty;
}