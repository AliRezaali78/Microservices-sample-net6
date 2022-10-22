using AutoMapper;

public class GetPlatformByIdHandler :
             IQueryHandler<GetPlatformByIdQuery, PlatformReadDto?>
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    public GetPlatformByIdHandler(IPlatformRepository repository, IMapper mapper)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    public async Task<PlatformReadDto?> Handle(GetPlatformByIdQuery request, CancellationToken cancellationToken)
    {
        var platformInDb = await _repository.GetAsync(request.Id);

        return (platformInDb == null) ? null : _mapper.Map<Platform, PlatformReadDto>(platformInDb);

    }
}
