using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using services.DTOs.PublisherDTOs;
using Services.Interfaces;

namespace main.Controllers;

[ApiController]
[Route("/api/publishers")]
public class PublisherController : Controller
{
    private readonly IPublisherService publisherService;

    public PublisherController(IPublisherService publisherService)
    {
        this.publisherService = publisherService;
    }
    
    /// <summary>
    /// Get information about publishers. Uses pagination.
    /// </summary>
    /// <param name="page">Query parameter containing chosen page number. Defaults to 0.</param>
    /// <param name="pageSize">Query parameter containing page size. Defaults to 20.</param>
    /// <returns>
    /// returns list of publishers associated with chosen title (200 OK).
    /// </returns>
    [HttpGet]
    public IActionResult Index([FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        var publisherLookupDtos = publisherService.GetPublishers(page, pageSize);
        return Ok(publisherLookupDtos);
    }
    
    /// <summary>
    /// Get detailed information about single publisher
    /// </summary>
    /// <param name="publisherId">Publisher identifier used in database</param>
    /// <returns>
    /// If publisher exists, returns information about author (200 OK),
    /// otherwise returns "not found" (404 Not Found).
    /// </returns>

    [HttpGet("{publisherId:int}")]
    public IActionResult Details(int publisherId)
    {
        var publisherDto =  publisherService.GetPublisher(publisherId);
        return publisherDto == null ? NotFound() : Ok(publisherDto);
    }
   
    /// <summary>
    /// Create new entry of publisher in database.
    /// </summary>
    /// <param name="createPublisherDto">New publisher data.</param>
    /// <returns>
    /// If createPublisherDto is correct, returns "created" (201 Created) with URL to new resource (Location response header) and newly created resource itself (JSON in response body),
    /// otherwise returns "bad request" (400 Bad Request).
    /// </returns>
    [HttpPost]
    public IActionResult Create([FromBody] CreatePublisherDTO? createPublisherDto)
    {
        if (createPublisherDto == null) return BadRequest();
        var id = publisherService.AddPublisher(createPublisherDto);
        var publisherDto = publisherService.GetPublisher(id);
        return Created("/api/publishers/" + id, publisherDto);
    }
    
    /// <summary>
    /// Update information about chosen publisher.
    /// </summary>
    /// <param name="publisherId">Publisher identifier used in database.</param>
    /// <param name="updatePublisherDto">Modified author data.</param>
    /// <returns>
    /// If publisherId is missing or updatePublisherDto is incorrect, returns "bad request" (400 Bad Request),
    /// otherwise if succeeds, returns updated publisher information (200 OK),
    /// otherwise returns "not found" (404 Not Found).
    /// </returns>
    [HttpPut("{publisherId:int?}")]
    public IActionResult Update(int? publisherId, [FromBody] CreatePublisherDTO? updatePublisherDto)
    {
        if (publisherId == null || updatePublisherDto == null) return BadRequest();
        var updateDto = new UpdatePublisherDTO()
        {
            PublisherId = (int)publisherId,
            Name = updatePublisherDto.Name,
        };
        var result = publisherService.UpdatePublisher(updateDto); 
        return result ? Ok(publisherService.GetPublisher((int)publisherId)) : NotFound();
    }
    
    /// <summary>
    /// Delete entry of chosen publisher.
    /// </summary>
    /// <param name="publisherId">Publisher identifier used in database.</param>
    /// <returns>
    /// If publisherId is missing, returns "bad request" (400 Bad Request),
    /// otherwise if operation is currently invalid, returns "conflict" (409 Conflict),
    /// otherwise if succeeds, returns "no content" (204 NoContent),
    /// otherwise returns "not found" (404 Not Found).
    /// </returns>
    [HttpDelete("{publisherId:int?}")]
    public IActionResult Delete(int? publisherId)
    {
        if (publisherId == null) return BadRequest();

        try
        {
            var result = publisherService.DeletePublisher((int)publisherId);
            return result ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}