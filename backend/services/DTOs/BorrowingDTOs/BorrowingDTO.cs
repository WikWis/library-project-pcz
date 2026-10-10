using services.DTOs.PieceDTOs;

namespace services.DTOs.BorrowingDTOs;

public class BorrowingDTO
{
    public int Id { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public PieceLookupDTO? Piece { get; set; }
    public int UserId { get; set; }
}