
using PortionWise.Models.Recipe;

namespace PortionWise.Configs
{
    class MappingConfig
    {
        public static void config(WebApplicationBuilder builder)
        {
            builder
                .Services
                .AddAutoMapper(
                    [
                        typeof(RecipeMapping)
                    ]
                );
        }
    }
}
