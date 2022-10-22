using AutoMapper;
using PlatformService;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();

        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(dest => dest.ExtenralId,
                opt => opt.MapFrom(dto => dto.Id));

        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(p => p.ExtenralId, opt =>
                opt.MapFrom(gp => gp.PlatformId))
            .ForMember(p => p.Name, opt =>
                opt.MapFrom(gp => gp.Name))
            .ForMember(p => p.Commands, opt =>
                opt.Ignore());
    }
}