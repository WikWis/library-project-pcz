using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model;
using Services.DTOs.TitleDTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public class TitleService : BaseService, ITitleService
    {
        public TitleService(IMapper mapper, MyDbContext dbContext) : base(mapper, dbContext)
        {
        }

        public TitleDTO? GetTitle(int titleId)
        {
            var title = _dbContext.Titles
                .Include(t => t.Authors)
                .Include(t => t.Publisher)
                .FirstOrDefault(t => t.TitleId == titleId);
            return _mapper.Map<TitleDTO>(title);
        }

        public IList<TitleLookupDTO> GetTitles(int page, int pageSize)
        {
            var pageBegin = page * pageSize;
            var titles = _dbContext.Titles
                .OrderBy(t => t.TitleId)
                .Skip(pageBegin)
                .Take(pageSize)
                .ToList();

            return _mapper.Map<IList<TitleLookupDTO>>(titles);
        }

        public IList<TitleLookupDTO> GetTitlesByAuthor(int page, int pageSize, int authorId)
        {
            var pageBegin = page * pageSize;
            var titles = _dbContext.Titles
                .Where(t => t.Authors.Any(a => a.AuthorId == authorId))
                .OrderBy(t => t.TitleId)
                .Skip(pageBegin)
                .Take(pageSize)
                .ToList();

            return _mapper.Map<IList<TitleLookupDTO>>(titles);
        }

        public int AddTitle(CreateTitleDTO createTitleDto)
        {
            var title = _mapper.Map<Title>(createTitleDto);

            if(createTitleDto.Authors.Count > 0)
            {
                var authorIds = createTitleDto.Authors
                    .Select(a => a.AuthorId)
                    .ToList();

                title.Authors = _dbContext.Authors
                    .Where(a => authorIds.Contains(a.AuthorId))
                    .ToList();
            }

            if (createTitleDto.Publisher != null)
            {
                title.Publisher = _dbContext.Publishers
                    .Find(createTitleDto.Publisher.PublisherId);
            }

            _dbContext.Titles.Add(title);
            _dbContext.SaveChanges();
            return title.TitleId;
        }

        public bool UpdateTitle(UpdateTitleDTO updateTitleDto)
        {
            var title = _dbContext.Titles
                .Include(t => t.Authors)
                .Include(t => t.Publisher)
                .FirstOrDefault(t => t.TitleId == updateTitleDto.TitleId);
            
            if (title == null) return false;

            _mapper.Map(updateTitleDto, title);
          
            var authorIds = updateTitleDto.Authors
                .Select(a => a.AuthorId)
                .ToList();
            title.Authors = _dbContext.Authors
                .Where(a => authorIds.Contains(a.AuthorId))
                .ToList();

            if(updateTitleDto.Publisher != null)
            {
                title.Publisher = _dbContext.Publishers
                    .Find(updateTitleDto.Publisher.PublisherId);
            }
            else
            {
                title.Publisher = null;
            }

            _dbContext.SaveChanges();
            return true;
        }

        public bool DeleteTitle(int titleId)
        {
            var title = _dbContext.Titles
                .Find(titleId);

            if (title == null) return false;

            _dbContext.Titles.Remove(title);
            _dbContext.SaveChanges();
            return true;
        }
    }
}