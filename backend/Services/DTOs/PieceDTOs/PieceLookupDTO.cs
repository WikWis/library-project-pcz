using Model;

namespace services.DTOs.PieceDTOs;

public class PieceLookupDTO
{
    public int Id { get; set; }
    public PieceStatus Status { get; set; } = PieceStatus.Free;
    public PieceState State { get; set; } = PieceState.Good;
}