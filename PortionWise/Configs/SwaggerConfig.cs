using Microsoft.OpenApi.Models;

namespace PortionWise.Configs
{
    class SwaggerConfig
    {
        public static void config(WebApplicationBuilder builder)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen();
            builder.Services.AddEndpointsApiExplorer();
        }
    }
}