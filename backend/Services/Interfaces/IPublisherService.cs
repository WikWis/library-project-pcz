using services.DTOs.PublisherDTOs;

namespace Services.Interfaces;

public interface IPublisherService
{
    /// <summary>
    /// Get detailed information about single publisher
    /// </summary>
    /// <param name="publisherId">Publisher identifier used in database</param>
    /// <returns>Information about publisher, including associated titles</returns>
    PublisherDTO? GetPublisher(int publisherId);
    
    /// <summary>
    /// Get information about every publisher. Uses pagination.
    /// </summary>
    /// <param name="page">Chosen page number. Starts with 0.</param>
    /// <param name="pageSize">Page size.</param>
    /// <returns>Information about publisher. Amount of entries is equal to pageSize.</returns>
    IList<PublisherLookupDTO> GetPublishers(int page, int pageSize);
    
    /// <summary>
    /// Create new entry of publisher in database.
    /// </summary>
    /// <param name="createPublisherDto">New publisher data.</param>
    /// <returns>Identifier of newly created entry.</returns>
    int AddPublisher(CreatePublisherDTO createPublisherDto);
    
    /// <summary>
    /// Update information about chosen publisher.
    /// </summary>
    /// <param name="updatePublisherDto">Modified publisher data.</param>
    /// <returns>Information if operation succeeded</returns>
    bool UpdatePublisher(UpdatePublisherDTO updatePublisherDto);
   
    /// <summary>
    /// Delete entry of chosen publisher.
    /// </summary>
    /// <param name="publisherId">Publisher identifier used in database</param>>
    /// <returns>Information if operation succeeded</returns>
    bool DeletePublisher(int publisherId);
}