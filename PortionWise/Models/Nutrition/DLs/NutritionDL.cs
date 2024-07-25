using System.Text.Json.Serialization;

namespace PortionWise.Models.Nutrition.DLs
{
    public class NutritionDL
    {
        [JsonPropertyName("items")]
        public required List<NutritionItem> Items { get; set; }
    }
}

public class NutritionItem
{
    [JsonPropertyName("sugar_g")]
    public double SugarGram { get; set; }

    [JsonPropertyName("fiber_g")]
    public double FiberGram { get; set; }

    [JsonPropertyName("serving_size_g")]
    public double ServingSize { get; set; }

    [JsonPropertyName("Sodium_mg")]
    public double SodiumMg { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("potassium_mg")]
    public double PotassiumMg { get; set; }

    [JsonPropertyName("fat_saturated_g")]
    public double FatSaturatedGram { get; set; }

    [JsonPropertyName("fat_total_g")]
    public double FatTotalGram { get; set; }

    [JsonPropertyName("calories")]
    public double Calories { get; set; }

    [JsonPropertyName("cholesterol_mg")]
    public double CholesterolMg { get; set; }

    [JsonPropertyName("protein_g")]
    public double ProteinGram { get; set; }

    [JsonPropertyName("carbohydrates_total_g")]
    public double CarbohydratesTotalGram { get; set; }
}
