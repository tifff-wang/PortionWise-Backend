
using PortionWise.Models.Ingredient;
using PortionWise.Models.Nutrition;
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
                        typeof(RecipeMapping),
                        typeof(IngredientMapping),
                        typeof(NutritionMapping)
                    ]
                );
        }
    }
}
