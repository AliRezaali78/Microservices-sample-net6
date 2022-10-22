using AutoMapper;
using Grpc.Net.Client;
using PlatformService;

public class GrpcPlatformDataClient : IPlatformDataClient
{
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    public GrpcPlatformDataClient(IConfiguration config, IMapper mapper)
    {
        _mapper = mapper;
        _config = config;
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        System.Console.WriteLine($"--> Calling GRPC Service {_config["GrpcPlatform"]}");
        var channel = GrpcChannel.ForAddress(_config["GrpcPlatform"]);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllPlatforms(request);
            return _mapper.Map<IEnumerable<Platform>>(reply.Platforms);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"--> Could not call GRPC Server {ex.Message}");
            return null;
        }
    }
}
