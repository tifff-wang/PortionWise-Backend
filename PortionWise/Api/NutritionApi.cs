using System.Text.Json;
using PortionWise.Models.Nutrition.DLs;

namespace PortionWise.Api
{
    public interface INutritionApi
    {
        Task<NutritionDL> GetNutritionInfo(string query);
    }

    public class NutritionApi : INutritionApi
    {
        private readonly HttpClient _httpClient;
        public static readonly string NutritionClient = "nutrition";

        public NutritionApi(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient(NutritionClient);
        }

        public async Task<NutritionDL> GetNutritionInfo(string query)
        {
            var url = _httpClient.BaseAddress + $"nutrition?query={query}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var nutritionData = JsonSerializer.Deserialize<NutritionDL>(responseBody);
            if (nutritionData == null)
            {
                throw new InvalidOperationException("Failed to deserialize the response.");
            }

            return nutritionData;
        }
    }
}
