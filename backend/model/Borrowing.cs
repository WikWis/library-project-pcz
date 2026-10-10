namespace model;

public class Borrowing
{
    public int Id { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int PieceId { get; set; }
    public Piece? Piece { get; set; }
}