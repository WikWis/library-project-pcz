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
    
    [HttpGet]
    public IActionResult Index([FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        var publisherLookupDtos = publisherService.GetPublishers(page, pageSize);
        return Ok(publisherLookupDtos);
    }
    [HttpGet("{publisherId:int}")]
    public IActionResult Details(int publisherId)
    {
        var publisherDto =  publisherService.GetPublisher(publisherId);
        return publisherDto == null ? NotFound() : Ok(publisherDto);
    }
    [HttpPost]
    public IActionResult Create([FromBody] CreatePublisherDTO? createPublisherDto)
    {
        if (createPublisherDto == null) return BadRequest();
        var id = publisherService.AddPublisher(createPublisherDto);
        var publisherDto = publisherService.GetPublisher(id);
        return Created("/api/publishers/" + id, publisherDto);
    }
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
    [HttpDelete("{publisherId:int?}")]
    public IActionResult Delete(int? publisherId)
    {
        if (publisherId == null) return BadRequest();
        var result = publisherService.DeletePublisher((int)publisherId); 
        return result ? NoContent() : NotFound();
    }
}