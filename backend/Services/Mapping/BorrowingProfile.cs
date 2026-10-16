using AutoMapper;
using Model;
using services.DTOs.BorrowingDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Mapping
{
    public class BorrowingProfile : Profile
    {
        public BorrowingProfile()
        {
            CreateMap<Borrowing, BorrowingDTO>();
        }
    }
}
