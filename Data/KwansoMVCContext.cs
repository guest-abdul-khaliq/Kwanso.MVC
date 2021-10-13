using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kwanso.MVC.Models;

namespace Kwanso.MVC.Data
{
    public class KwansoMVCContext : DbContext
    {
        public KwansoMVCContext (DbContextOptions<KwansoMVCContext> options)
            : base(options)
        {
        }

        public DbSet<Kwanso.MVC.Models.Login> Login { get; set; }
    }
}
