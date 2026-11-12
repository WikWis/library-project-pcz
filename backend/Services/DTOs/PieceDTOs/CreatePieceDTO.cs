using Model;
using services.DTOs.BorrowingDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.DTOs.PieceDTOs
{
    public class CreatePieceDTO
    {
        
        public int TitleId { get; set; }

        [EnumDataType(typeof(PieceStatus), ErrorMessage = "Nieprawidłowy status egzemplarza.")]
        public PieceStatus Status { get; set; } = PieceStatus.Free;
        
        [EnumDataType(typeof(PieceState), ErrorMessage = "Nieprawidłowy stan egzemplarza.")]
        public PieceState State { get; set; } = PieceState.Good;
    }
}
