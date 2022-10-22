using Microsoft.EntityFrameworkCore;

public static class Datas
{
    public static void Seed(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
            var dataClient = scope.ServiceProvider.GetRequiredService<IPlatformDataClient>();
            Process(repo, dataClient);
        }
    }

    private static void Process(ICommandRepo repo, IPlatformDataClient dataClient)
    {

        var platforms = dataClient.GetAllPlatforms();

        System.Console.WriteLine("--> Seeding new platforms...");

        foreach (var p in platforms)
        {
            if (repo.ExternalPlatformExists(p.ExtenralId))
                continue;

            repo.CreatePlatform(p);
            repo.SaveChanges();
        }

    }


}