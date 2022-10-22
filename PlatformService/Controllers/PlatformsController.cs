using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{

    private readonly IMediator _mediator;

    public PlatformsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlatforms()
    {
        var platformDtos = await _mediator.Send(new GetAllPlatformsQuery());
        return Ok(platformDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlatfromById(int id)
    {
        var platformDto = await _mediator.Send(new GetPlatformByIdQuery(id));
        if (platformDto == null) return NotFound();

        return Ok(platformDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlatform(PlatformCreateDto createDto)
    {
        var dto = await _mediator.Send(new CreatePlatformCommand(createDto));

        return CreatedAtAction(nameof(GetPlatfromById), new { id = dto.Id }, dto);
    }


}