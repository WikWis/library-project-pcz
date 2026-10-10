using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
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
        public int PieceId { get; set; }
        public virtual Title? Title { get; set; }
        public PieceStatus Status { get; set; } = PieceStatus.Free;
        public PieceState State { get; set; } = PieceState.Good;
        public virtual ICollection<Borrowing> BorrowingHistory { get; set; } = [];
    }
}
