using Microsoft.EntityFrameworkCore;
using PortionWise.Database;

namespace PortionWise.Configs
{
    class DatabaseConfig
    {
        public static void config(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDBContext>();
        }
    }
}