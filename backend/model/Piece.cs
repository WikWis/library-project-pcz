namespace model;

public enum PieceStatus
{
    Free,
    Reserved,
    Borrowed,
    InRepair,
    Archived
}

public enum PieceState
{
    Good,
    Damaged,
    Unusable
}

public class Piece
{
    public int Id { get; set; }
    public PieceStatus Status { get; set; } = PieceStatus.Free;
    public PieceState State { get; set; } = PieceState.Good;
    public ICollection<Borrowing> BorrowingHistory { get; set; } = [];
}