using Microsoft.EntityFrameworkCore;
using Pamwork.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamwork.DAL.Concrete
{
    public class PamworkDbContext:DbContext
    {
        public PamworkDbContext(DbContextOptions<PamworkDbContext> options)
            : base(options)
        {
        }

        public DbSet<Note>  Notes { get; set; }


    }
}
