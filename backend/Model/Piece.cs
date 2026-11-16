using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public enum PieceState
    {
        Good,
        Damaged,
        Unusable
    }

    public class Piece
    {
        public int PieceId { get; set; }
        public virtual Title? Title { get; set; }
        public PieceState State { get; set; } = PieceState.Good;
        public virtual ICollection<Borrowing> BorrowingHistory { get; set; } = [];
    }
}
