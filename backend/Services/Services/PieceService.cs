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
            if (pieceId <= 0)
                throw new ArgumentException("Id egzemplarza musi być większy od zera.", nameof(pieceId));

            var piece = _dbContext.Pieces
                .AsNoTracking()
                .Include(p => p.BorrowingHistory)
                .Include(p => p.Title)
                .FirstOrDefault(p => p.PieceId == pieceId);

            return piece == null ? null : _mapper.Map<PieceDTO>(piece);
        }

        public IList<PieceLookupDTO> GetPieces(int page, int pageSize)
        {
            var pageBegin = page * pageSize;
            var pieces = _dbContext.Pieces
                .AsNoTracking()
                .OrderBy(p => p.PieceId)
                .Skip(pageBegin)
                .Take(pageSize)
                .ToList();

            return _mapper.Map<IList<PieceLookupDTO>>(pieces);
        }

        public IList<PieceLookupDTO> GetPiecesByTitle(int page, int pageSize, int titleId)
        {
            if (titleId <= 0)
                throw new ArgumentException("Id tytułu musi być większy od zera.", nameof(titleId));

            var pageBegin = page * pageSize;
            var pieces = _dbContext.Pieces
                .AsNoTracking()
                .Where(p => p.Title != null && p.Title.TitleId == titleId)
                .OrderBy(p => p.PieceId)
                .Skip(pageBegin)
                .Take(pageSize)
                .ToList();

            return _mapper.Map<IList<PieceLookupDTO>>(pieces);
        }

        public int AddPiece(int titleId, CreatePieceDTO createPieceDto)
        {
            if (createPieceDto == null)
                throw new ArgumentNullException(nameof(createPieceDto), "Dane egzemplarza nie mogą być puste.");

            if (createPieceDto.TitleId <= 0)
                throw new ArgumentException("Id tytułu musi być większe od 0.", nameof(createPieceDto.TitleId));

            if (!Enum.IsDefined(typeof(PieceStatus), createPieceDto.Status))
                throw new ArgumentException("Nieprawidłowy status egzemplarza.", nameof(createPieceDto.Status));

            if (!Enum.IsDefined(typeof(PieceState), createPieceDto.State))
                throw new ArgumentException("Nieprawidłowy stan egzemplarza.", nameof(createPieceDto.State));

            var title = _dbContext.Titles.Find(titleId);
            if (title == null)
            {
                throw new KeyNotFoundException($"Nie znaleziono tytułu o Id {titleId}.");
            }

            var piece = _mapper.Map<Piece>(createPieceDto);
            piece.Title = title;

            try
            {
                _dbContext.Pieces.Add(piece);
                _dbContext.SaveChanges();
                return piece.PieceId;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Nie udało się dodać egzemplarza do bazy danych.", ex);
            }
        }

        public bool UpdatePiece(UpdatePieceDTO updatePieceDto)
        {
            if (updatePieceDto == null)
                throw new ArgumentNullException(nameof(updatePieceDto), "Dane nie mogą być puste.");

            if (updatePieceDto.PieceId <= 0)
                throw new ArgumentException("Id egzemplarza musi być większe od 0.", nameof(updatePieceDto.PieceId));

            if (!Enum.IsDefined(typeof(PieceStatus), updatePieceDto.Status))
                throw new ArgumentException("Nieprawidłowy status egzemplarza.", nameof(updatePieceDto.Status));

            if (!Enum.IsDefined(typeof(PieceState), updatePieceDto.State))
                throw new ArgumentException("Nieprawidłowy stan egzemplarza.", nameof(updatePieceDto.State));

            var piece = _dbContext.Pieces
                .FirstOrDefault(p => p.PieceId == updatePieceDto.PieceId);

            if (piece == null)
            {
                throw new KeyNotFoundException($"Nie znaleziono egzemplarza o Id {updatePieceDto.PieceId}.");
            }

            _mapper.Map(updatePieceDto, piece);

            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("Egzemplarz został w międzyczasie zmodyfikowany przez innego użytkownika.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Nie udało się zaktualizować danych egzemplarza w bazie.", ex);
            }
        }

        public bool DeletePiece(int pieceId)
        {
            if (pieceId <= 0)
                throw new ArgumentException("Id egzemplarza musi być większy od zera.", nameof(pieceId));

            var piece = _dbContext.Pieces
                .Find(pieceId);

            if (piece == null) throw new KeyNotFoundException($"Nie znaleziono egzemplarza o ID {pieceId}.");

            if (_dbContext.Borrowings.Any(b => b.PieceId == pieceId))
                throw new InvalidOperationException("Nie można usunąć egzemplarza, ponieważ posiada przypisane wypożyczenia.");

            try
            {
                _dbContext.Pieces.Remove(piece);
                _dbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Nie można usunąć egzemplarza z powodu powiązań w bazie danych.", ex);
            }
        }
    }
}
