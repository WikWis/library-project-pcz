using Model;
using services.DTOs.BorrowingDTOs;

namespace Services.Interfaces;

public interface IBorrowingService
{
    IList<BorrowingDTO> GetBorrowingsByUser(int page, int pageSize, string userId);
    IList<BorrowingDTO> GetBorrowingsByStatus(int page, int pageSize, BorrowingStatus status);
    
    //For reader
    BorrowingDTO? ReserveBorrowing(CreateBorrowingDTO createBorrowingDto);
    bool CancelBorrowing(int borrowingId, string userId); // delete jeśli stan reserved
    
    //For librarian
    BorrowingDTO? CreateBorrowing(CreateBorrowingDTO createBorrowingDto); //wprost wypożyczenie on-site
    BorrowingDTO? ConfirmBorrowing(int borrowingId); //startdate ustaw na aktualny czas
    BorrowingDTO? ReturnBorrowing(int borrowingId); //enddate ustaw na aktualny czas
    bool DeleteBorrowing(int borrowingId);

}