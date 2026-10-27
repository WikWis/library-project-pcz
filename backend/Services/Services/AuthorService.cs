using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model;
using services.DTOs.AuthorDTOs;
using Services.Interfaces;

namespace Services.Services;

public class AuthorService : BaseService, IAuthorService
{
    public AuthorService(IMapper mapper, MyDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public AuthorDTO? GetAuthor(int authorId)
    {
        var author = _dbContext.Authors
            .Include(a => a.Titles)
            .FirstOrDefault(a => a.AuthorId == authorId);
        return _mapper.Map<AuthorDTO>(author);
    }

    public IList<AuthorLookupDTO> GetAuthors(int page, int pageSize)
    {
        var pageBegin = page * pageSize;
        IList<Author> authors = _dbContext.Authors
            .OrderBy(a=>a.AuthorId)
            .Skip(pageBegin)
            .Take(pageSize)
            .ToList();
        
        return _mapper.Map<IList<AuthorLookupDTO>>(authors);
    }

    public IList<AuthorLookupDTO> GetAuthorsByTitle(int page, int pageSize, int titleId)
    {
        var title = _dbContext.Titles
            .Include(t => t.Authors)
            .FirstOrDefault(t => t.TitleId == titleId);

        if(title == null) return [];
        
        var pageBegin = page * pageSize;
        return _mapper.Map<List<AuthorLookupDTO>>(title.Authors.Skip(pageBegin).Take(pageSize));
    }

    public int AddAuthor(CreateAuthorDTO createAuthorDto)
    {
        var author = _mapper.Map<Author>(createAuthorDto);
        _dbContext.Authors.Add(author);
        _dbContext.SaveChanges();
        return author.AuthorId;
    }

    public bool UpdateAuthor(UpdateAuthorDTO updateAuthorDto)
    {
        var author = _dbContext.Authors.FirstOrDefault(a => a.AuthorId == updateAuthorDto.AuthorId);
        if (author == null) return false;

        author.Name = updateAuthorDto.Name;
        author.Surname = updateAuthorDto.Surname;
        _dbContext.SaveChanges();
        return true;
    }

    public bool DeleteAuthor(int authorId)
    { 
        var author = _dbContext.Authors
            .Include(a => a.Titles)
            .FirstOrDefault(a => a.AuthorId == authorId);
        if (author == null) return false;
        if(author.Titles.Count > 0) throw new InvalidOperationException("Cannot delete an Author which has a Title assigned to it");

        _dbContext.Authors.Remove(author);
        _dbContext.SaveChanges();
        return true;
    }
}