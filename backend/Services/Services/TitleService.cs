using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public async Task<TitleDTO> GetTitleAsync(int titleId)
        {
            if (titleId <= 0) 
            {
                throw new ArgumentException("Id tytułu musi być większy od zera.", nameof(titleId));
            }


            var title = await _dbContext.Titles
                .Include(t => t.Authors)
                .Include(t => t.Publisher)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TitleId == titleId);

            if (title == null)
            {
                throw new KeyNotFoundException($"Nie znaleziono tytułu o ID: {titleId}.");
            }

            return _mapper.Map<TitleDTO>(title);
        }

        public async Task<IList<TitleLookupDTO>> GetTitlesAsync(int page, int pageSize)
        {
            var pageBegin = page * pageSize;
            var titles = await _dbContext.Titles
                .AsNoTracking()
                .OrderBy(t => t.TitleId)
                .Skip(pageBegin)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IList<TitleLookupDTO>>(titles);

        }

        public async Task<IList<TitleLookupDTO>> GetTitlesByAuthorAsync(int page, int pageSize, int authorId)
        {
            if (authorId <= 0)
            {
                throw new ArgumentException("Identyfikator autora musi być większy od zera.", nameof(authorId));
            }

            bool authorExists = await _dbContext.Authors.AnyAsync(a => a.AuthorId == authorId);
            if (!authorExists)
            {
                throw new KeyNotFoundException($"Autor o ID {authorId} nie istnieje w bazie danych.");
            }

            var pageBegin = page * pageSize;
            var titles = await _dbContext.Titles
                .AsNoTracking()
                .Where(t => t.Authors.Any(a => a.AuthorId == authorId))
                .OrderBy(t => t.TitleId)
                .Skip(pageBegin)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IList<TitleLookupDTO>>(titles);
        }

        public async Task<IList<TitleLookupDTO>> GetTitlesByPublisherAsync(int page, int pageSize, int publisherId)
        {
            if (publisherId <= 0)
            {
                throw new ArgumentException("Id wydawnictwa musi być większe od 0.", nameof(publisherId));
            }

            bool publisherExists = await _dbContext.Publishers.AnyAsync(p => p.PublisherId == publisherId);
            if (!publisherExists)
            {
                throw new KeyNotFoundException($"Wydawnictwo o ID {publisherId} nie istnieje w bazie danych.");
            }

            var pageBegin = page * pageSize;
            var titles = await _dbContext.Titles
                .AsNoTracking()
                .Where(t => t.PublisherId == publisherId)
                .OrderBy(t => t.TitleId)
                .Skip(pageBegin)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IList<TitleLookupDTO>>(titles);
        }

        public async Task<int> AddTitleAsync(CreateTitleDTO createTitleDto)
        {
            if (createTitleDto == null)
            {
                throw new ArgumentNullException(nameof(createTitleDto), "Dane do utworzenia tytułu nie mogą być puste.");
            }

            if (createTitleDto.DateOfPublishing > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ArgumentException("Data publikacji nie może być datą z przyszłości.");
            }

            // czy ISBN jest unikalny
            if (!string.IsNullOrWhiteSpace(createTitleDto.ISBN))
            {
                var cleanIsbn = createTitleDto.ISBN.Trim();
                bool isbnExists = await _dbContext.Titles.AnyAsync(t => t.ISBN == cleanIsbn);
                if (isbnExists)
                {
                    throw new InvalidOperationException($"Tytuł z numerem ISBN '{cleanIsbn}' już istnieje.");
                }
            }

            //czy wydawca istnieje
            if (createTitleDto.Publisher != null)
            {
                bool publisherExists = await _dbContext.Publishers
                    .AnyAsync(p => p.PublisherId == createTitleDto.Publisher.PublisherId);

                if (!publisherExists)
                {
                    throw new KeyNotFoundException($"Wydawca o ID {createTitleDto.Publisher.PublisherId} nie istnieje.");
                }
            }

            // czy autorzy istnieją
            List<Author> authors = new();
            if (createTitleDto.Authors != null && createTitleDto.Authors.Any())
            {
                var authorIds = createTitleDto.Authors
                    .Select(a => a.AuthorId)
                    .Distinct()
                    .ToList();

                authors = await _dbContext.Authors
                    .Where(a => authorIds.Contains(a.AuthorId))
                    .ToListAsync();

                // jeśli któryś z autorów nie istnieje
                if (authors.Count != authorIds.Count)
                {
                    throw new KeyNotFoundException("1 lub więcej podanych autorów nie istnieje w bazie danych.");
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
            await _dbContext.SaveChangesAsync();
            return title.TitleId;
        }

        public async Task UpdateTitleAsync(UpdateTitleDTO updateTitleDto)
        {
            if (updateTitleDto == null)
            {
                throw new ArgumentNullException(nameof(updateTitleDto), "Dane do aktualizacji tytułu nie mogą być puste.");
            }

            if (updateTitleDto.TitleId <= 0)
            {
                throw new ArgumentException("Identyfikator tytułu musi być większy od zera.", nameof(updateTitleDto.TitleId));
            }

            var title = await _dbContext.Titles
                .Include(t => t.Authors)
                .FirstOrDefaultAsync(t => t.TitleId == updateTitleDto.TitleId);

            if (title == null)
            {
                throw new KeyNotFoundException($"Nie znaleziono tytułu o ID: {updateTitleDto.TitleId}.");
            }

            if (updateTitleDto.DateOfPublishing > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ArgumentException("Data publikacji nie może być datą z przyszłości.");
            }

            // czy ISBN jest unikalny
            if (!string.IsNullOrWhiteSpace(updateTitleDto.ISBN))
            {
                var cleanIsbn = updateTitleDto.ISBN.Trim();
                bool isbnExists = await _dbContext.Titles
                    .AnyAsync(t => t.ISBN == cleanIsbn && t.TitleId != updateTitleDto.TitleId);

                if (isbnExists)
                {
                    throw new InvalidOperationException($"Inny tytuł posiada już numer ISBN '{cleanIsbn}'.");
                }
            }

            if (updateTitleDto.Publisher != null)
            {
                bool publisherExists = await _dbContext.Publishers
                    .AnyAsync(p => p.PublisherId == updateTitleDto.Publisher.PublisherId);

                if (!publisherExists)
                {
                    throw new KeyNotFoundException($"Wydawca o ID {updateTitleDto.Publisher.PublisherId} nie istnieje.");
                }
            }

            List<Author> newAuthors = new();
            if (updateTitleDto.Authors != null && updateTitleDto.Authors.Any())
            {
                var authorIds = updateTitleDto.Authors
                    .Select(a => a.AuthorId)
                    .Distinct()
                    .ToList();

                newAuthors = await _dbContext.Authors
                    .Where(a => authorIds.Contains(a.AuthorId))
                    .ToListAsync();

                if (newAuthors.Count != authorIds.Count)
                {
                    throw new KeyNotFoundException("1 lub więcej podanych autorów nie istnieje.");
                }
            }

            _mapper.Map(updateTitleDto, title);
            title.PublisherId = updateTitleDto.Publisher?.PublisherId;

            title.Authors.Clear();
            foreach (var author in newAuthors)
            {
                title.Authors.Add(author);
            }

            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteTitleAsync(int titleId)
        {
            if (titleId <= 0)
            {
                throw new ArgumentException("Identyfikator tytułu musi być większy od zera.", nameof(titleId));
            }

            int deletedRows = await _dbContext.Titles
                .Where(t => t.TitleId == titleId)
                .ExecuteDeleteAsync();

            if (deletedRows == 0)
            {
                throw new KeyNotFoundException($"Nie znaleziono tytułu o ID {titleId} do usunięcia.");
            }
        }
    }
}