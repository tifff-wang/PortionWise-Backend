using PortionWise.Api;

namespace PortionWise.Configs
{
    public class HttpClientConfig
    {
        public static void config(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient(
                "nutrition",
                c =>
                {
                    var apiKey = ApiKeyStore.NutritionApiKey;
                    c.BaseAddress = new Uri("https://api.calorieninjas.com/v1/");
                    c.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
                }
            );
        }
    }
}
