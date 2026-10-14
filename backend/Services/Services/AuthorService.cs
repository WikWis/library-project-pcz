using AutoMapper;
using DAL;
using Model;
using services.DTOs.AuthorDTOs;
using Services.Interfaces;

namespace Services.Services;

public class AuthorService : BaseService, IAuthorService
{
    public AuthorService(IMapper mapper, MyDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public AuthorDTO? GetAuthor(int id)
    {
        var author = _dbContext.Authors.Find(id);
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
        throw new NotImplementedException();
    }

    public int AddAuthor(CreateAuthorDTO dto)
    {
        var author = _mapper.Map<Author>(dto);
        _dbContext.Authors.Add(author);
        _dbContext.SaveChanges();
        return author.AuthorId;
    }

    public bool UpdateAuthor(UpdateAuthorDTO dto)
    {
        var author = _dbContext.Authors.FirstOrDefault(a => a.AuthorId == dto.AuthorId);
        if (author == null) return false;

        author.Name = dto.Name;
        author.Surname = dto.Surname;
        _dbContext.SaveChanges();
        return true;
    }

    public bool DeleteAuthor(int id)
    {
        var author = _dbContext.Authors.Find(id);
        if (author == null) return false;

        _dbContext.Authors.Remove(author);
        _dbContext.SaveChanges();
        return true;
    }
}