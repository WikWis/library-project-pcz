using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.DTOs.PieceDTOs
{
    public class UpdatePieceDTO
    {
        
        public int PieceId { get; set; }

        [EnumDataType(typeof(PieceStatus), ErrorMessage = "Nieprawidłowy status egzemplarza.")]
        public PieceStatus Status { get; set; }

        [EnumDataType(typeof(PieceState), ErrorMessage = "Nieprawidłowy stan egzemplarza.")]
        public PieceState State { get; set; }
    }
}
