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

    /// <summary>
    /// Get detailed information about single publisher
    /// </summary>
    /// <param name="publisherId">Publisher identifier used in database</param>
    /// <returns>Information about publisher, including associated titles</returns>
    public PublisherDTO? GetPublisher(int publisherId)
    {
        var publisher = _dbContext.Publishers.Find(publisherId);
        return _mapper.Map<PublisherDTO>(publisher);
    }

    /// <summary>
    /// Get information about every publisher. Uses pagination.
    /// </summary>
    /// <param name="page">Chosen page number. Starts with 0.</param>
    /// <param name="pageSize">Page size.</param>
    /// <returns>Information about publisher. Amount of entries is equal to pageSize.</returns>
    public IList<PublisherLookupDTO> GetPublishers(int page, int pageSize)
    {
        var pageBegin = page * pageSize;
        IList<Publisher> authors = _dbContext.Publishers
            .OrderBy(a => a.PublisherId)
            .Skip(pageBegin)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<IList<PublisherLookupDTO>>(authors);
    }

    /// <summary>
    /// Create new entry of publisher in database.
    /// </summary>
    /// <param name="createPublisherDto">New publisher data.</param>
    /// <returns>Identifier of newly created entry.</returns>
    public int AddPublisher(CreatePublisherDTO createPublisherDto)
    {
        var publisher = _mapper.Map<Publisher>(createPublisherDto);
        _dbContext.Publishers.Add(publisher);
        _dbContext.SaveChanges();
        return publisher.PublisherId;
    }

    /// <summary>
    /// Update information about chosen publisher.
    /// </summary>
    /// <param name="updatePublisherDto">Modified publisher data.</param>
    /// <returns>Information if operation succeeded</returns>
    public bool UpdatePublisher(UpdatePublisherDTO updatePublisherDto)
    {
        var publisher = _dbContext.Publishers.FirstOrDefault(a => a.PublisherId == updatePublisherDto.PublisherId);
        if (publisher == null) return false;

        publisher.Name = updatePublisherDto.Name;
        _dbContext.SaveChanges();
        return true;
    }

    /// <summary>
    /// Delete entry of chosen publisher.
    /// </summary>
    /// <param name="publisherId">Publisher identifier used in database</param>>
    /// <returns>Information if operation succeeded</returns>
    public bool DeletePublisher(int publisherId)
    {
        var publisher = _dbContext.Publishers
            .Include(p => p.Titles)
            .FirstOrDefault(p => p.PublisherId == publisherId);
        if (publisher == null) return false;
        if (publisher.Titles.Count > 0)
            throw new InvalidOperationException("Cannot delete an Publisher which has a Title assigned to it");

        _dbContext.Publishers.Remove(publisher);
        _dbContext.SaveChanges();
        return true;
    }
}