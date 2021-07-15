using System.Linq;

namespace Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.PriceLists.Any()) return;
        }
    }
}
