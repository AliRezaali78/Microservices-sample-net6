using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services

// if (builder.Environment.IsDevelopment())
// {
//     System.Console.WriteLine("--> Using InMem Db");
//     builder.Services.AddDbContext<AppDbContext>(opt =>
//     {
//         opt.UseInMemoryDatabase("InMem");
//     });
// }
// else
// {
System.Console.WriteLine("--> Using Sql Server Db");
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConnection"));
});
// }



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllers();
builder.AddSwaggerService();
builder.RegisterDependencies();

// APP
var app = builder.Build();

app.SetUpDevelopmentEnv();

// app.UseHttpsRedirection();
app.UseAuthorization();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapGrpcService<GrpcPlatformService>();
    endpoints.MapGet("/protos/platforms.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
    });
});


Datas.Seed(app, app.Environment.IsProduction());

app.Run();
