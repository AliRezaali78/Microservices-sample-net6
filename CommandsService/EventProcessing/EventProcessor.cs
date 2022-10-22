using System.Text.Json;
using AutoMapper;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopedFactory;
    private readonly IMapper _mapper;

    // IServiceScope is scoped 
    // VS
    // ISeviceScopeFactory is singleton
    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopedFactory = scopeFactory;
        _mapper = mapper;
    }
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        // we can use poly morphism here instead
        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
            default:
                break;
        }

    }

    private void AddPlatform(string message)
    {
        using (var scope = _scopedFactory.CreateAsyncScope())
        {
            // because ICommandRepo is scoped base, we get it through here because scope factory is singleton
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
            try
            {
                var platform = _mapper.Map<Platform>(platformPublishedDto);
                if (repo.ExternalPlatformExists(platform.ExtenralId))
                {
                    System.Console.WriteLine("--> Platform already exists...");
                    return;
                }

                repo.CreatePlatform(platform);
                repo.SaveChanges();

                System.Console.WriteLine("Platform added !");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"--> Could not add platform to DB {ex.Message}");
            }

        }
    }

    private EventType DetermineEvent(string message)
    {
        System.Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);
        switch (eventType.Event)
        {
            case "Platform_Published":
                System.Console.WriteLine("--> Platform Published Event Detected");
                return EventType.PlatformPublished;

            default:
                System.Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}