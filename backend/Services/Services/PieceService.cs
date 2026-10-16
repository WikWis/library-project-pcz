using AutoMapper;
using DAL;
using Model;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.PieceDTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public class PieceService : BaseService, IPieceService
    {
        public PieceService(IMapper mapper, MyDbContext dbContext) : base(mapper, dbContext)
        {

        }

        public PieceDTO? GetPiece(int pieceId)
        {
            var piece = _dbContext.Pieces
                .Include(p => p.BorrowingHistory)
                .Include(p => p.Title)
                .FirstOrDefault(p => p.PieceId == pieceId);

            return _mapper.Map<PieceDTO>(piece);
        }

        public IList<PieceLookupDTO> GetPieces(int page, int pageSize)
        {
            var pageBegin = page * pageSize;
            var pieces = _dbContext.Pieces
                .OrderBy(p => p.PieceId)
                .Skip(pageBegin)
                .Take(pageSize)
                .ToList();

            return _mapper.Map<IList<PieceLookupDTO>>(pieces);
        }

        public IList<PieceLookupDTO> GetPiecesByTitle(int page, int pageSize, int titleId)
        {
            var pageBegin = page * pageSize;
            var pieces = _dbContext.Pieces
                .Where(p => p.Title != null && p.Title.TitleId == titleId)
                .OrderBy(p => p.PieceId)
                .Skip(pageBegin)
                .Take(pageSize)
                .ToList();

            return _mapper.Map<IList<PieceLookupDTO>>(pieces);
        }

        public int AddPiece(int titleId, CreatePieceDTO createPieceDto)
        {
            var title = _dbContext.Titles.Find(titleId);
            if (title == null)
            {
                return 0;
            }

            var piece = _mapper.Map<Piece>(createPieceDto);
            piece.Title = title;

            _dbContext.Pieces.Add(piece);
            _dbContext.SaveChanges();
            return piece.PieceId;
        }

        public bool UpdatePiece(UpdatePieceDTO updatePieceDto)
        {
            var piece = _dbContext.Pieces
                .FirstOrDefault(p => p.PieceId == updatePieceDto.PieceId);

            if(piece == null)
            {
                return false;
            }

            _mapper.Map(updatePieceDto, piece);
            _dbContext.SaveChanges();

            return true;
        }

        public bool DeletePiece(int pieceId)
        {
            var piece = _dbContext.Pieces.Find(pieceId);

            if (piece == null) return false;

            _dbContext.Pieces.Remove(piece);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
