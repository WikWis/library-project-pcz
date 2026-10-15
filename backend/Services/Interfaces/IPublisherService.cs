using services.DTOs.PublisherDTOs;

namespace Services.Interfaces;

public interface IPublisherService
{
    PublisherDTO? GetPublisher(int publisherId);
    IList<PublisherLookupDTO> GetPublishers(int page, int pageSize);
    
    int AddPublisher(CreatePublisherDTO createPublisherDto);
    bool UpdatePublisher(UpdatePublisherDTO updatePublisherDto);
    bool DeletePublisher(int publisherId);
}