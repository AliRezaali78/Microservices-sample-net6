using AutoMapper;

public class GetAllPlatformsHandler : IQueryHandler<GetAllPlatformsQuery, IEnumerable<PlatformReadDto>>
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    public GetAllPlatformsHandler(IPlatformRepository repository, IMapper mapper)
    {
        this._mapper = mapper;
        this._repository = repository;
    }
    public async Task<IEnumerable<PlatformReadDto>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
    {
        return (await _repository.GetAllAsync()).Select(_mapper.Map<Platform, PlatformReadDto>);
    }
}
