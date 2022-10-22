using Microsoft.EntityFrameworkCore;

public static class Datas
{
    public static void Seed(WebApplication app, bool isProduction)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            Process(context, isProduction);
        }
    }

    private static void Process(AppDbContext context, bool isProduction)
    {
        // if production we will migrate database.
        if (isProduction)
            MigrateDb(context);

        if (!isProduction)
            if (!context.Platforms.Any())
            {
                System.Console.WriteLine("Seeding Data");
                context.Platforms.AddRange(
                    new Platform("name-1", "test-1", "2000"),
                    new Platform("name-2", "test-2", "3000"),
                    new Platform("name-3", "test-3", "4000")
                );
                context.SaveChanges();
                return;
            }
            else
                System.Console.WriteLine("We already have data !");
    }

    private static void MigrateDb(AppDbContext context)
    {
        System.Console.WriteLine("--> Attempting to apply migrations...");
        try
        {
            context.Database.Migrate();
        }
        catch (Exception ex)
        {

            System.Console.WriteLine($"--> Could not run migrations: {ex.Message}");
        }
    }
}