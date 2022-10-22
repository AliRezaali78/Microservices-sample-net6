using AutoMapper;
using Grpc.Core;
using PlatformService;

public class GrpcPlatformService : PlatformService.GrpcPlatform.GrpcPlatformBase
{
    private IPlatformRepository _repository;
    private IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository repo, IMapper mapper)
    {
        _repository = repo;
        _mapper = mapper;
    }

    public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var platformResponse = new PlatformResponse();
        var platforms = await _repository.GetAllAsync();
        foreach (var plat in platforms)
        {
            var grpcPlatformModel = _mapper.Map<GrpcPlatformModel>(plat);
            platformResponse.Platforms.Add(grpcPlatformModel);
        }

        return platformResponse;
    }
}