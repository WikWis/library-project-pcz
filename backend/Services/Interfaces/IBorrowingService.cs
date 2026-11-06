using services.DTOs.BorrowingDTOs;

namespace Services.Interfaces;

public interface IBorrowingService
{
    IList<BorrowingDTO> GetBorrowingsByUser(int page, int pageSize, string userId);
    BorrowingDTO? AddBorrowing(CreateBorrowingDTO createBorrowingDto);
    BorrowingDTO? UpdateBorrowing(UpdateBorrowingDTO updateBorrowingDto);
    bool DeleteBorrowing(int borrowingId);
}