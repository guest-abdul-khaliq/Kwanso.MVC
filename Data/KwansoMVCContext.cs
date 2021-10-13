using Microsoft.EntityFrameworkCore;

namespace Kwanso.MVC.Data
{
    public class KwansoMVCContext : DbContext
    {
        public KwansoMVCContext (DbContextOptions<KwansoMVCContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Login> Login { get; set; }
    }
}
