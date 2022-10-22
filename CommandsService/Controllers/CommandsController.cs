using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICommandRepo _repository;

    public CommandsController(ICommandRepo repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    [ServiceFilter(typeof(PlatformExistsFilter))]
    [HttpGet]
    public IActionResult GetCommandsForPlatform(int platformId)
    {
        System.Console.WriteLine($"--> Getting Commands for platform id {platformId}");

        // if (!_repository.PlatformExists(platformId))
        //     return NotFound();

        var commandDtos = _repository.GetCommandsForPlatform(platformId)
            .Select(_mapper.Map<CommandReadDto>);

        return Ok(commandDtos);
    }

    [ServiceFilter(typeof(PlatformExistsFilter))]
    [HttpGet("{commandId}")]
    public IActionResult GetCommandForPlatform(int platformId, int commandId)
    {
        System.Console.WriteLine($"--> Getting Single Command {commandId} for platform id {platformId}");

        var command = _repository.GetCommand(platformId, commandId);
        if (command == null)
            return NotFound();

        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    [ServiceFilter(typeof(PlatformExistsFilter))]
    [HttpPost]
    public IActionResult CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        System.Console.WriteLine($"--> Create Command for platform id {platformId}");

        var command = _mapper.Map<Command>(commandDto);

        _repository.CreateCommand(platformId, command);
        _repository.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);
        return CreatedAtAction(nameof(GetCommandForPlatform), new { platformId, commandId = commandReadDto.Id }, commandReadDto);
    }
}