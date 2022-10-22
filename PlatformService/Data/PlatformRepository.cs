using Microsoft.EntityFrameworkCore;

public class PlatformRepository : EfRepository<Platform>, IPlatformRepository
{
    public PlatformRepository(AppDbContext context) : base(context)
    {
    }
}