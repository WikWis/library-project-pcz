using Model;
using services.DTOs.BorrowingDTOs;

namespace Services.DTOs.PieceDTOs;

public class PieceDTO
{
    public int PieceId { get; set; }
    public int TitleId { get; set; }
    public string? TitleName { get; set; }
    public PieceStatus Status { get; set; }
    public PieceState State { get; set; }
    public IList<BorrowingDTO> BorrowingHistory { get; set; } = [];
}