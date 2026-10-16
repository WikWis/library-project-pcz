using AutoMapper;
using Model;
using Services.DTOs.PieceDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Mapping
{
    public class PieceProfile : Profile
    {
        public PieceProfile()
        {
            CreateMap<Piece, PieceDTO>()
                .ForMember(d => d.PieceId, o => o
                    .MapFrom(s => s.PieceId))
                .ForMember(d => d.TitleId, o => o
                    .MapFrom(s => s.Title != null 
                        ? s.Title.TitleId
                        : 0))
                .ForMember(d => d.BorrowingHistory, o => o
                    .MapFrom(s => s.BorrowingHistory));

            CreateMap<Piece, PieceLookupDTO>()
                .ForMember(d => d.PieceId, o => o
                    .MapFrom(s => s.PieceId))
                .ForMember(d => d.TitleId, o => o
                    .MapFrom(s => s.Title != null
                        ? s.Title.TitleId
                        : 0));

            CreateMap<CreatePieceDTO, Piece>()
                .ForMember(d => d.PieceId, o => o.Ignore())
                .ForMember(d => d.Title, o => o.Ignore())
                .ForMember(d => d.BorrowingHistory, o => o.Ignore());

            CreateMap<UpdatePieceDTO, Piece>()
                .ForMember(d => d.PieceId, o => o.Ignore())
                .ForMember(d => d.Title, o => o.Ignore())
                .ForMember(d => d.BorrowingHistory, o => o.Ignore());
        }
    }
}
