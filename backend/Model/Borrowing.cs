using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Borrowing
    {
        public int BorrowingId { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public int PieceId { get; set; }
        public virtual Piece? Piece { get; set; }
    }
}
