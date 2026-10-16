using Model;

namespace Services.DTOs.PieceDTOs;

public class PieceLookupDTO
{
    public int PieceId { get; set; }
    public int TitleId { get; set; }
    public PieceStatus Status { get; set; }
    public PieceState State { get; set; }
}