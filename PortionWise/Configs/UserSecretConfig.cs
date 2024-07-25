namespace PortionWise.Configs
{
    public class UserSecretConfig
    {
        public static void config(WebApplicationBuilder builder)
        {
            builder.Configuration.AddUserSecrets<Program>();
            var apiKey = builder.Configuration["Api_Key"];
            ApiKeyStore.NutritionApiKey = apiKey;
        }
    }
}

public static class ApiKeyStore
{
    public static string? NutritionApiKey { get; set; }
}
