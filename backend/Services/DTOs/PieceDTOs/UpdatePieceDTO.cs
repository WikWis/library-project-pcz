using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs.PieceDTOs
{
    public class UpdatePieceDTO
    {
        public int PieceId { get; set; }
        public PieceStatus Status { get; set; }
        public PieceState State { get; set; }
    }
}
