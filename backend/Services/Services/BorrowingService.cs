using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model;
using services.DTOs.BorrowingDTOs;
using Services.Interfaces;
using Services.Services;

namespace Services.Services;

public class BorrowingService : BaseService, IBorrowingService
{
    public BorrowingService(IMapper mapper, MyDbContext dbContext) : base(mapper, dbContext)
    {
    }

    /// <summary>
    /// Get information about every borrowing of chosen user. Uses pagination.
    /// </summary>
    /// <param name="page">Chosen page number. Starts with 0.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="userId">User identifier used in database.</param>
    /// <returns>Information about borrowings. Amount of entries is equal to pageSize.</returns>
    public IList<BorrowingDTO> GetBorrowingsByUser(int page, int pageSize, string userId)
    {
        var pageBegin = page * pageSize;
        var borrowings = _dbContext.Borrowings
            .Include(b => b.Piece)
            .Where(b => b.UserId == userId)
            .Skip(pageBegin)
            .Take(pageSize)
            .OrderBy(b => b.BorrowingId)
            .ToList();
        return  _mapper.Map<IList<BorrowingDTO>>(borrowings);
    }

    public IList<BorrowingDTO> GetBorrowingsByStatus(int page, int pageSize, BorrowingStatus status)
    {
        var pageBegin = page * pageSize;
        var borrowings = _dbContext.Borrowings
            .Include(b => b.Piece)
            .Where(b => b.Status == status)
            .Skip(pageBegin)
            .Take(pageSize)
            .OrderBy(b => b.BorrowingId)
            .ToList();
        return  _mapper.Map<IList<BorrowingDTO>>(borrowings);    }

    public BorrowingDTO? ReserveBorrowing(ReserveBorrowingDTO reserveBorrowingDto)
    {
        throw new NotImplementedException();
    }

    public BorrowingDTO? ChangeBorrowingStatus(int borrowingId, BorrowingStatus status)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Delete entry of chosen borrowing.
    /// </summary>
    /// <param name="borrowingId">Borrowing identifier used in database</param>
    /// <returns>Information if operation succeeded</returns>
    public bool DeleteBorrowing(int borrowingId)
    {
        var borrowing = _dbContext.Borrowings
            .FirstOrDefault(b => b.BorrowingId == borrowingId);
        if (borrowing == null) return false;
        
        _dbContext.Borrowings.Remove(borrowing);
        _dbContext.SaveChanges();
        return true; 
    }
}