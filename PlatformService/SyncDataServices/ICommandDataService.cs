public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto platformReadDto);
}