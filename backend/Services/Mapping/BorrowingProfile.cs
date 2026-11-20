using AutoMapper;
using Model;
using services.DTOs.BorrowingDTOs;

namespace Services.Mapping
{
    public class BorrowingProfile : Profile
    {
        public BorrowingProfile()
        {
            CreateMap<Borrowing, BorrowingDTO>();
            CreateMap<CreateBorrowingDTO, Borrowing>()
                .ForMember(d => d.BorrowingId, o => o.Ignore())
                .ForMember(d => d.Status, o => o.Ignore())
                .ForMember(d => d.Piece, o => o.Ignore())
                .ForMember(d => d.User, o => o.Ignore());
        }
    }
}