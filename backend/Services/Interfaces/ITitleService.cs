using Services.DTOs.TitleDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface ITitleService
    {
        Task<TitleDTO> GetTitleAsync(int publisherId);
        Task<IList<TitleLookupDTO>> GetTitlesAsync(int page, int pageSize);
        Task<IList<TitleLookupDTO>> GetTitlesByAuthorAsync(int page, int pageSize, int authorId);
        Task<IList<TitleLookupDTO>> GetTitlesByPublisherAsync(int page, int pageSize, int publisherId);

        Task<int> AddTitleAsync(CreateTitleDTO createTitleDto);
        Task UpdateTitleAsync(UpdateTitleDTO updateTitleDto);
        Task DeleteTitleAsync(int titleId);
    }
}
