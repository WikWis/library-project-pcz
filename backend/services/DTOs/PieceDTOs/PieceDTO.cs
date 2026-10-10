using model;
using services.DTOs.BorrowingDTOs;

namespace services.DTOs.PieceDTOs;

public class PieceDTO
{
    public int Id { get; set; }
    public PieceStatus Status { get; set; } = PieceStatus.Free;
    public PieceState State { get; set; } = PieceState.Good;
    public IList<BorrowingDTO> BorrowingHistory { get; set; } = [];
}