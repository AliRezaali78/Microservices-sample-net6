using MediatR;
public static class DependencyExtensions
{
    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
        builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataService>();

        builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

        builder.Services.AddGrpc();
    }
}
