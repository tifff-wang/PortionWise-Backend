using System.Text.Json;
using PortionWise.Models.Nutrition;
using PortionWise.Models.Nutrition.DLs;

namespace PortionWise.Api
{
    public interface INutritionApi
    {
        Task<TotalNutritionDL> GetNutritionInfo(string query);
    }

    public class NutritionApi : INutritionApi
    {
        private readonly HttpClient _httpClient;

        public NutritionApi(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("nutrition");
        }

        public async Task<TotalNutritionDL> GetNutritionInfo(string query)
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

                return SumNutritionInfo(nutritionData);
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

        public TotalNutritionDL SumNutritionInfo(NutritionDL nutritionData)
        {
            TotalNutritionDL totalNutrition = new TotalNutritionDL
            {
                SugarGram = Math.Round(nutritionData.Items.Sum(item => item.SugarGram), 1),
                FiberGram = Math.Round(nutritionData.Items.Sum(item => item.FiberGram), 1),
                ServingSize = nutritionData.Items.Sum(item => item.ServingSize),
                SodiumMg = Math.Round(nutritionData.Items.Sum(item => item.SodiumMg), 1),
                PotassiumMg = Math.Round(nutritionData.Items.Sum(item => item.PotassiumMg), 1),
                FatSaturatedGram = Math.Round(
                    nutritionData.Items.Sum(item => item.FatSaturatedGram),
                    1
                ),
                FatTotalGram = Math.Round(nutritionData.Items.Sum(item => item.FatTotalGram), 1),
                Calories = Math.Round(nutritionData.Items.Sum(item => item.Calories)),
                CholesterolMg = Math.Round(nutritionData.Items.Sum(item => item.CholesterolMg), 1),
                ProteinGram = Math.Round(nutritionData.Items.Sum(item => item.ProteinGram), 1),
                CarbohydratesTotalGram = Math.Round(
                    nutritionData.Items.Sum(item => item.CarbohydratesTotalGram),
                    1
                )
            };

            return totalNutrition;
        }
    }
}
