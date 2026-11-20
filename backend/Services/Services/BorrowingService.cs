using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model;
using services.DTOs.AuthorDTOs;
using services.DTOs.BorrowingDTOs;
using Services.Interfaces;
using Services.Mapping;

namespace Services.Services;

public class BorrowingService : BaseService, IBorrowingService
{
    public BorrowingService(IMapper mapper, MyDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public IList<BorrowingDTO> GetBorrowingsByUser(int page, int pageSize, string userId)
    {
        var pageBegin = page * pageSize;
        var borrowings = _dbContext.Borrowings
            .Where(b => b.UserId == userId)
            .Include(b => b.Piece)
            .OrderBy(b => b.BorrowingId)
            .Skip(pageBegin)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<IList<BorrowingDTO>>(borrowings);
    }

    public IList<BorrowingDTO> GetBorrowingsByStatus(int page, int pageSize, BorrowingStatus status)
    {
        var pageBegin = page * pageSize;
        var borrowings = _dbContext.Borrowings
            .Where(b => b.Status == status)
            .Include(b => b.Piece)
            .OrderBy(b => b.BorrowingId)
            .Skip(pageBegin)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<IList<BorrowingDTO>>(borrowings);
    }

    public BorrowingDTO? ReserveBorrowing(CreateBorrowingDTO createBorrowingDto)
    {
        if (createBorrowingDto.EndDate <= createBorrowingDto.StartDate)
            throw new InvalidOperationException("End date must be at least one day after start date");

        var piece = _dbContext.Pieces
            .Include(p => p.BorrowingHistory)
            .FirstOrDefault(p => p.PieceId == createBorrowingDto.PieceId);
        if(piece  == null) return null;
        
        if(piece.BorrowingHistory.Any(b => b.Status
               is BorrowingStatus.Reserved
               or BorrowingStatus.Borrowed
               ))
            throw new InvalidOperationException("Piece isn't currently available for reserving");
        
        var borrowing = _mapper.Map<Borrowing>(createBorrowingDto);
        borrowing.Status = BorrowingStatus.Reserved;
        _dbContext.Borrowings.Add(borrowing);
        _dbContext.SaveChanges();
        return _mapper.Map<BorrowingDTO>(borrowing);
    }

    public bool CancelBorrowing(int borrowingId, string userId)
    {
        var borrowing = _dbContext.Borrowings
            .FirstOrDefault(b => b.BorrowingId == borrowingId);
        if (borrowing == null) return false;
        
        if(borrowing.UserId != userId)
            throw new InvalidOperationException("Borrowing does not belong to the current user");
        if(borrowing.Status != BorrowingStatus.Reserved)
            throw new InvalidOperationException("Cannot cancel the borrowing that is not just reserved");
        
        _dbContext.Borrowings.Remove(borrowing);
        _dbContext.SaveChanges();

        return true;
    }

    public BorrowingDTO? CreateBorrowing(CreateBorrowingDTO createBorrowingDto)
    {
        if (!_dbContext.Users.Any(u=>u.Id == createBorrowingDto.UserId))
            throw new InvalidOperationException("Chosen user does not exist");
        
        var piece = _dbContext.Pieces
            .Include(p => p.BorrowingHistory)
            .FirstOrDefault(p => p.PieceId == createBorrowingDto.PieceId);
        if(piece  == null) return null;
        
        if(piece.BorrowingHistory.Any(b => b.Status
               is BorrowingStatus.Reserved
               or BorrowingStatus.Borrowed
           ))
            throw new InvalidOperationException("Piece isn't currently available for borrowing");

        var borrowing = _mapper.Map<Borrowing>(createBorrowingDto);
        borrowing.Status = BorrowingStatus.Borrowed;
        borrowing.StartDate = DateOnly.FromDateTime(DateTime.Now);
        if (borrowing.EndDate <= borrowing.StartDate)
            throw new InvalidOperationException("End date must be at least one day after start date");
        _dbContext.Borrowings.Add(borrowing);
        _dbContext.SaveChanges();
        return _mapper.Map<BorrowingDTO>(borrowing);
    }

    public BorrowingDTO? ConfirmBorrowing(int borrowingId)
    {
        var borrowing = _dbContext.Borrowings
            .Include(b => b.Piece)
            .FirstOrDefault(b => b.BorrowingId == borrowingId);
        if(borrowing == null) return null;
        
        if(borrowing.Status != BorrowingStatus.Reserved)
            throw new InvalidOperationException("Borrowing isn't reserved");

        borrowing.Status = BorrowingStatus.Borrowed;
        borrowing.StartDate = DateOnly.FromDateTime(DateTime.Now);
        if (borrowing.EndDate <= borrowing.StartDate)
            throw new InvalidOperationException("End date must be at least one day after start date");
        _dbContext.SaveChanges();
        
        return _mapper.Map<BorrowingDTO>(borrowing);
    }

    public BorrowingDTO? ReturnBorrowing(int borrowingId)
    {
        var borrowing = _dbContext.Borrowings
            .Include(b => b.Piece)
            .FirstOrDefault(b => b.BorrowingId == borrowingId);
        if(borrowing == null) return null;
        
        if(borrowing.Status != BorrowingStatus.Borrowed)
            throw new InvalidOperationException("Borrowing isn't borrowed");
        
        borrowing.EndDate = DateOnly.FromDateTime(DateTime.Now);
        borrowing.Status = BorrowingStatus.Returned;
        _dbContext.SaveChanges();
        
        return _mapper.Map<BorrowingDTO>(borrowing);
    }

    public bool DeleteBorrowing(int borrowingId)
    {
        var borrowing = _dbContext.Borrowings
            .FirstOrDefault(b => b.BorrowingId == borrowingId);
        if(borrowing == null) return false;
        
        if(borrowing.Status is not (BorrowingStatus.Reserved or BorrowingStatus.Returned))
            throw new InvalidOperationException("Cannot delete borrowing unless it is reserved or returned");
        
        _dbContext.Borrowings.Remove(borrowing);
        _dbContext.SaveChanges();
        return true;
    }
}