using Model;
using services.DTOs.BorrowingDTOs;

namespace Services.Interfaces;

public interface IBorrowingService
{
    /// <summary>
    /// Get information about every borrowing of chosen user. Uses pagination.
    /// </summary>
    /// <param name="page">Chosen page number. Starts with 0.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="userId">User identifier used in database.</param>
    /// <returns>Information about borrowings. Amount of entries is equal to pageSize.</returns>
    IList<BorrowingDTO> GetBorrowingsByUser(int page, int pageSize, string userId);
    
    /// <summary>
    /// Get information about every borrowing having chosen status. Uses pagination.
    /// </summary>
    /// <param name="page">Chosen page number. Starts with 0.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="status">Chosen status of borrowing.</param>
    /// <returns>Information about borrowings. Amount of entries is equal to pageSize.</returns>
    IList<BorrowingDTO> GetBorrowingsByStatus(int page, int pageSize, BorrowingStatus status);
    
    /// <summary>
    /// Create new entry of borrowing in database.
    /// </summary>
    /// <param name="reserveBorrowingDto">New borrowing data.</param>
    /// <returns>Identifier of newly created entry.</returns>
    BorrowingDTO? ReserveBorrowing(ReserveBorrowingDTO reserveBorrowingDto);

    /// <summary>
    /// Change status of chosen borrowing.
    /// </summary>
    /// <param name="borrowingId">Borrowing identifier used in database</param>
    /// <param name="status">New borrowing status</param>
    /// <returns>Identifier of updated created entry.</returns>
    BorrowingDTO? ChangeBorrowingStatus(int  borrowingId, BorrowingStatus status);

    /// <summary>
    /// Delete entry of chosen borrowing.
    /// </summary>
    /// <param name="borrowingId">Borrowing identifier used in database</param>
    /// <returns>Information if operation succeeded</returns>
    bool DeleteBorrowing(int borrowingId);
}