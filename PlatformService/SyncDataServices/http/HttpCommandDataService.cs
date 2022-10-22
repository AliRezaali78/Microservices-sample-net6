using System.Text.Json;

public class HttpCommandDataService : ICommandDataClient
{
    private readonly HttpClient _httpClinet;
    private readonly IConfiguration _config;

    public HttpCommandDataService(HttpClient httpClient, IConfiguration config)
    {
        _httpClinet = httpClient;
        _config = config;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize<PlatformReadDto>(platformReadDto),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClinet.PostAsync($"{_config["CommandServicePlatformsUri"]}", httpContent);
        if (response.IsSuccessStatusCode)
            System.Console.WriteLine("--> Sync POST to CommandService was OK !");
        else
            System.Console.WriteLine("--> Error: Sync POST to CommandService was NOT OK !");
    }
}