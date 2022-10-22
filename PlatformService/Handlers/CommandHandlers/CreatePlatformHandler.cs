using AutoMapper;

public class CreatePlatformHandler : ICommandHandler<CreatePlatformCommand, PlatformReadDto>
{
    private readonly IPlatformRepository _repository;
    private readonly IMessageBusClient _messageBusClient;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _dataClient;

    public CreatePlatformHandler(
        IPlatformRepository repository,
        IMapper mapper,
        ICommandDataClient dataClient,
        IMessageBusClient messageBusClient)
    {
        _mapper = mapper;
        _dataClient = dataClient;
        _repository = repository;
        _messageBusClient = messageBusClient;
    }
    public async Task<PlatformReadDto> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        var platform = _mapper.Map<PlatformCreateDto, Platform>(request.CreateDto);
        await _repository.AddAsync(platform);
        await _repository.SaveChangesAsync();

        var readDto = _mapper.Map<Platform, PlatformReadDto>(platform);

        // Synchroness
        await SendSync(readDto);

        // Send Asynchroness
        SendAsync(readDto);


        return readDto;
    }

    private void SendAsync(PlatformReadDto readDto)
    {
        try
        {
            var platformPublishDto = _mapper.Map<PlatformPublishedDto>(readDto);
            // document magic strings
            platformPublishDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishDto);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
        }
    }

    private async Task SendSync(PlatformReadDto readDto)
    {
        try
        {
            await _dataClient.SendPlatformToCommand(readDto);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
        }
    }
}