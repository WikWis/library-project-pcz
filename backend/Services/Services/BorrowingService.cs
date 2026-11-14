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
            .Where(b => b.UserId == userId)
            .Skip(pageBegin)
            .Take(pageSize)
            .OrderBy(b => b.BorrowingId)
            .ToList();
        return  _mapper.Map<IList<BorrowingDTO>>(borrowings);
    }

    /// <summary>
    /// Create new entry of borrowing in database.
    /// </summary>
    /// <param name="createBorrowingDto">New borrowing data.</param>
    /// <returns>Identifier of newly created entry.</returns>
    public BorrowingDTO? AddBorrowing(CreateBorrowingDTO createBorrowingDto)
    {
        if (!_dbContext.Users.Any(u => u.Id == createBorrowingDto.UserId))
            throw new InvalidOperationException("User not found");
        if(!_dbContext.Pieces.Any(p => p.PieceId == createBorrowingDto.PieceId))
            throw new InvalidOperationException("Piece not found");
        if(CanBorrowDuring(createBorrowingDto.PieceId, createBorrowingDto.StartDate, createBorrowingDto.EndDate))
            throw new InvalidOperationException("Piece already borrowed during this time");
        
        var borrowing = _mapper.Map<Borrowing>(createBorrowingDto);
        _dbContext.Borrowings.Add(borrowing);
        _dbContext.SaveChanges();
        return _mapper.Map<BorrowingDTO>(borrowing);
    }

    /// <summary>
    /// Update information about chosen borrowing.
    /// </summary>
    /// <param name="updateBorrowingDto">Modified borrowing data.</param>
    /// <returns>Information if operation succeeded</returns>
    public BorrowingDTO? UpdateBorrowing(UpdateBorrowingDTO updateBorrowingDto)
    {
        var borrowing = _dbContext.Borrowings
            .FirstOrDefault(a => a.BorrowingId == updateBorrowingDto.BorrowingId);
        if (borrowing == null) return null;
        if (CanBorrowDuring(borrowing.PieceId, updateBorrowingDto.StartDate, updateBorrowingDto.EndDate))
            throw new InvalidOperationException("Piece already borrowed during this time");

        borrowing.StartDate = updateBorrowingDto.StartDate;
        borrowing.EndDate = updateBorrowingDto.EndDate;
        _dbContext.SaveChanges();
        return _mapper.Map<BorrowingDTO>(borrowing);
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

    /// <summary>
    /// Checks if chosen piece is available during chosen period.
    /// </summary>
    /// <param name="pieceId">Piece identifier used in database.</param>
    /// <param name="startDate">Beginning of chosen period</param>
    /// <param name="endDate">End of chosen period</param>
    /// <returns>returns if is available</returns>
    private bool CanBorrowDuring(int pieceId, DateOnly startDate, DateOnly endDate)
    {
        if(startDate >= endDate) return false;

        return _dbContext.Borrowings
            .Where(b => b.PieceId == pieceId)
            .Any(b => b.StartDate > endDate || b.EndDate < startDate);
    }
}