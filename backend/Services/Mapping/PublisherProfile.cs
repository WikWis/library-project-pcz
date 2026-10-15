using AutoMapper;
using Model;
using services.DTOs.PublisherDTOs;

namespace Services.Mapping;

public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        CreateMap<Publisher, PublisherLookupDTO>();
        CreateMap<Publisher, PublisherDTO>()
            .ForMember(d => d.Titles, o => o.Ignore());
        CreateMap<CreatePublisherDTO, Publisher>()
            .ForMember(d => d.Titles, o => o.Ignore())
            .ForMember(d => d.PublisherId, o => o.Ignore());
    }
}