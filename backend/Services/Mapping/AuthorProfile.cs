using AutoMapper;
using Model;
using services.DTOs.AuthorDTOs;

namespace Services.Mapping;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorLookupDTO>();
        CreateMap<Author, AuthorDTO>();
        CreateMap<CreateAuthorDTO, Author>()
            .ForMember(d=>d.AuthorId, o=>o.Ignore())
            .ForMember(d=>d.Titles, o=>o.Ignore());
    }
}