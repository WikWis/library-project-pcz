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
    /// Create new entry of borrowing in database.
    /// </summary>
    /// <param name="createBorrowingDto">New borrowing data.</param>
    /// <returns>Identifier of newly created entry.</returns>
    BorrowingDTO? AddBorrowing(CreateBorrowingDTO createBorrowingDto);
  
    /// <summary>
    /// Update information about chosen borrowing.
    /// </summary>
    /// <param name="updateBorrowingDto">Modified borrowing data.</param>
    /// <returns>Information if operation succeeded</returns>
    BorrowingDTO? UpdateBorrowing(UpdateBorrowingDTO updateBorrowingDto);
   
    /// <summary>
    /// Delete entry of chosen borrowing.
    /// </summary>
    /// <param name="borrowingId">Borrowing identifier used in database</param>
    /// <returns>Information if operation succeeded</returns>
    bool DeleteBorrowing(int borrowingId);
}