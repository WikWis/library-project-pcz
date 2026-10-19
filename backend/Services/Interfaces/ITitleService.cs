using Services.DTOs.TitleDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface ITitleService
    {
        TitleDTO? GetTitle(int publisherId);
        IList<TitleLookupDTO> GetTitles(int page, int pageSize);
        IList<TitleLookupDTO> GetTitlesByAuthor(int page, int pageSize, int authorId);

        int AddTitle(CreateTitleDTO createTitleDto);
        bool UpdateTitle(UpdateTitleDTO updateTitleDto);
        bool DeleteTitle(int titleId);
    }
}
