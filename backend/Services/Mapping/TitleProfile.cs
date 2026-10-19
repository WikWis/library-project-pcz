using AutoMapper;
using Model;
using Services.DTOs.TitleDTOs;

namespace Services.Mapping;

public class TitleProfile : Profile
{
    public TitleProfile()
    {
        CreateMap<Title, TitleLookupDTO>()
            .ForMember(g => g.Genres, o => o
                .MapFrom(s => string.IsNullOrWhiteSpace(s.Genres)
                    ? new List<string>()
                    : s.Genres.Split('/', StringSplitOptions.RemoveEmptyEntries)
                    .ToList()));

        CreateMap<Title, TitleDTO>()
            .ForMember(g => g.Genres, o => o
                .MapFrom(s => string.IsNullOrWhiteSpace(s.Genres)
                    ? new List<string>()
                    : s.Genres.Split('/', StringSplitOptions.RemoveEmptyEntries)
                    .ToList()));

        CreateMap<CreateTitleDTO, Title>()
            .ForMember(d => d.TitleId, o => o.Ignore())
            .ForMember(d => d.PublisherId, o => o.Ignore())
            .ForMember(d => d.Genres, o => o
                .MapFrom(s => s.Genres != null
                    ? string.Join("/", s.Genres)
                    : string.Empty))
            .ForMember(d => d.Authors, o => o.Ignore())
            .ForMember(d => d.Publisher, o => o.Ignore());

        CreateMap<UpdateTitleDTO, Title>()
            .ForMember(d => d.TitleId, o => o.Ignore())
            .ForMember(d => d.PublisherId, o => o.Ignore())
            .ForMember(d => d.Genres, o => o
                .MapFrom(s => s.Genres != null
                    ? string.Join("/", s.Genres)
                    : string.Empty))
            .ForMember(d => d.Authors, o => o.Ignore())
            .ForMember(d => d.Publisher, o => o.Ignore());
    }
}