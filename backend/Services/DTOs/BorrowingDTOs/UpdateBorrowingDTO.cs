namespace services.DTOs.BorrowingDTOs;

public class UpdateBorrowingDTO
{
    public int? BorrowingId { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}