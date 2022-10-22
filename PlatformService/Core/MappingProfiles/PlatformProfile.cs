using AutoMapper;
using PlatformService;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishedDto>();

        // GrpcPlatformModel is generated when we build the project
        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(grpc => grpc.PlatformId,
                opt => opt.MapFrom(p => p.Id))
             .ForMember(grpc => grpc.Name,
                opt => opt.MapFrom(p => p.Name))
             .ForMember(grpc => grpc.Publisher,
                opt => opt.MapFrom(p => p.Publisher));
    }
}