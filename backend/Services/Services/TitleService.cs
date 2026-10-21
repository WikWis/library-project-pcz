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

        public int? AddTitle(CreateTitleDTO createTitleDto)
        {
            if (createTitleDto.Publisher != null)
            {
                var publisherExists = _dbContext.Publishers
                    .Any(p => p.PublisherId == createTitleDto.Publisher.PublisherId);

                if (!publisherExists)
                {
                    return null;
                }
            }

            List<Author> authors = new();
            if (createTitleDto.Authors != null && createTitleDto.Authors.Any())
            {
                var authorIds = createTitleDto.Authors
                    .Select(a => a.AuthorId)
                    .ToList();

                authors = _dbContext.Authors
                    .Where(a => authorIds.Contains(a.AuthorId))
                    .ToList();

                //If author doesn;t exist
                if (authors.Count != authorIds.Distinct().Count())
                {
                    return null;
                }
            }

            var title = new Title
            {
                Name = createTitleDto.Name,
                DateOfPublishing = createTitleDto.DateOfPublishing,
                ISBN = createTitleDto.ISBN,
                Genres = createTitleDto.Genres != null && createTitleDto.Genres.Any()
                    ? string.Join("/", createTitleDto.Genres)
                    : string.Empty,
                PublisherId = createTitleDto.Publisher?.PublisherId,
                Authors = authors
            };

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

            if (updateTitleDto.Publisher != null)
            {
                var publisherExists = _dbContext.Publishers
                    .Any(p => p.PublisherId == updateTitleDto.Publisher.PublisherId);

                if (!publisherExists)
                {
                    return false;
                }
                title.PublisherId = updateTitleDto.Publisher.PublisherId;
            }
            else
            {
                title.PublisherId = null;
            }
            title.Publisher = null;

            title.Name = updateTitleDto.Name;
            title.DateOfPublishing = updateTitleDto.DateOfPublishing;
            title.ISBN = updateTitleDto.ISBN;
            title.Genres = updateTitleDto.Genres != null && updateTitleDto.Genres.Any()
                ? string.Join("/", updateTitleDto.Genres)
                : string.Empty;
          
            var authorIds = updateTitleDto.Authors
                .Select(a => a.AuthorId)
                .ToList();
            var newAuthors = _dbContext.Authors
                .Where(a => authorIds.Contains(a.AuthorId))
                .ToList();

            if (newAuthors.Count != authorIds.Distinct().Count())
            {
                return false;
            }

            title.Authors.Clear();
            foreach (var author in newAuthors)
            {
                title.Authors.Add(author);
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