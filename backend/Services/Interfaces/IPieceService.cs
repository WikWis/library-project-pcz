using Services.DTOs.PieceDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IPieceService
    {
        PieceDTO? GetPiece(int pieceId);
        IList<PieceLookupDTO> GetPieces(int page, int pageSize);
        IList<PieceLookupDTO> GetPiecesByTitle(int page, int pageSize, int titleId);

        int AddPiece(int titleOd, CreatePieceDTO createPieceDto);
        bool UpdatePiece(UpdatePieceDTO updatePieceDTO);
        bool DeletePiece(int pieceId);
    }
}
