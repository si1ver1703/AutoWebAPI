using ITSTEPASPNET.Model;
using Microsoft.EntityFrameworkCore;

namespace ITSTEPASPNET.Data
{
    public class AutoContext : DbContext
    {

        public AutoContext(DbContextOptions<AutoContext> options) : base(options)
        {
        }

        public DbSet<Model.Auto> autos { get; set; }

    }
}
