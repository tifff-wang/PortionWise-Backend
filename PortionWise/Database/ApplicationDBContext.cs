using Microsoft.EntityFrameworkCore;

namespace PortionWise.Database
{
    public class ApplicationDBContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ApplicationDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("DefaultSQLConnection"));
        }
    }

}
