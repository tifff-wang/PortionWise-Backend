namespace PortionWise.Configs
{
    public class UserSecretConfig
    {
        public static void config(IConfiguration configuration)
        {
            var apiKey = configuration["Api_Key"];
            ApiKeyStore.NutritionApiKey = apiKey;
        }
    }
}

public static class ApiKeyStore
{
    public static string? NutritionApiKey { get; set; }
}
