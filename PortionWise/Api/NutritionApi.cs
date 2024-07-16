using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PortionWise.Models.Nutrition;

namespace PortionWise.Api
{
    public interface INutritionApi
    {
        Task<NutritionDL> GetNutritionInfo(string query);
    }

    public class NutritionApi : INutritionApi
    {
        private readonly HttpClient _httpClient;

        // private readonly string _apiKey;

        public NutritionApi(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("nutrition");
            // _httpClient = factory.CreateClient();
            // _apiKey =
            //     ApiKeyStore.ApiKey
            //     ?? throw new InvalidOperationException("API key is not initialized.");
        }

        public async Task<NutritionDL> GetNutritionInfo(string query)
        {
            var url = _httpClient.BaseAddress + $"nutrition?query={query}";
            Console.WriteLine(url);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                Console.WriteLine(response);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                var nutritionData = JsonSerializer.Deserialize<NutritionDL>(responseBody);
                if (nutritionData == null)
                {
                    throw new InvalidOperationException("Failed to deserialize the response.");
                }
                return nutritionData;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                throw;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Deserialization error: {e.Message}");
                throw;
            }
        }
    }
}
