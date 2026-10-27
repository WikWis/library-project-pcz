using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;
using Model;
using services.DTOs.PublisherDTOs;
using Services.Interfaces;

namespace Services.Services;

public class PublisherService : BaseService, IPublisherService
{
    public PublisherService(IMapper mapper, MyDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public PublisherDTO? GetPublisher(int publisherId)
    {
        var publisher = _dbContext.Publishers.Find(publisherId);
        return _mapper.Map<PublisherDTO>(publisher);
    }

    public IList<PublisherLookupDTO> GetPublishers(int page, int pageSize)
    {
        var pageBegin = page * pageSize;
        IList<Publisher> authors = _dbContext.Publishers
            .OrderBy(a=>a.PublisherId)
            .Skip(pageBegin)
            .Take(pageSize)
            .ToList();
        
        return _mapper.Map<IList<PublisherLookupDTO>>(authors);
    }

    public int AddPublisher(CreatePublisherDTO createPublisherDto)
    {
        var publisher = _mapper.Map<Publisher>(createPublisherDto);
        _dbContext.Publishers.Add(publisher);
        _dbContext.SaveChanges();
        return publisher.PublisherId;
    }

    public bool UpdatePublisher(UpdatePublisherDTO updatePublisherDto)
    {
        var publisher = _dbContext.Publishers.FirstOrDefault(a => a.PublisherId == updatePublisherDto.PublisherId);
        if (publisher == null) return false;

        publisher.Name = updatePublisherDto.Name;
        _dbContext.SaveChanges();
        return true;
    }

    public bool DeletePublisher(int publisherId)
    {
        var publisher = _dbContext.Publishers
            .Include(p => p.Titles)
            .FirstOrDefault(p => p.PublisherId == publisherId);
        if (publisher == null) return false;
        if (publisher.Titles.Count > 0) throw new InvalidOperationException("Cannot delete an Publisher which has a Title assigned to it");
        
        _dbContext.Publishers.Remove(publisher);
        _dbContext.SaveChanges();
        return true;    }
}