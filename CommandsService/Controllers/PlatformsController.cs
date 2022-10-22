using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepo _repository;
    private readonly IMapper _mapper;

    public PlatformsController(ICommandRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetPlatforms()
    {
        System.Console.WriteLine("--> Getting Platforms Fromo Commands Service");
        var platformDtos = (_repository.GetAllPlatforms())
                        .Select(_mapper.Map<PlatformReadDto>);

        return Ok(platformDtos);
    }


    [HttpPost]
    public IActionResult TestInboundConnection()
    {
        System.Console.WriteLine("---> Inbound POST # Command Service");

        return Ok("Inbound test of from Platform Service");
    }
}