using Model;
using services.DTOs.BorrowingDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTOs.PieceDTOs
{
    public class CreatePieceDTO
    {
        public int TitleId { get; set; }
        public PieceStatus Status { get; set; } = PieceStatus.Free;
        public PieceState State { get; set; } = PieceState.Good;
    }
}
