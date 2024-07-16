using System.Text.Json.Serialization;

namespace PortionWise.Models.Nutrition
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
    public double Fiber_g { get; set; }

    [JsonPropertyName("serving_size_g")]
    public double Serving_size_g { get; set; }

    [JsonPropertyName("Sodium_mg")]
    public double osdium_mg { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("potassium_mg")]
    public double Potassium_mg { get; set; }

    [JsonPropertyName("fat_saturated_g")]
    public double Fat_saturated_g { get; set; }

    [JsonPropertyName("fat_total_g")]
    public double Fat_total_g { get; set; }

    [JsonPropertyName("calories")]
    public double Calories { get; set; }

    [JsonPropertyName("cholesterol_mg")]
    public double Cholesterol_mg { get; set; }

    [JsonPropertyName("protein_g")]
    public double Protein_g { get; set; }

    [JsonPropertyName("carbohydrates_total_g")]
    public double Carbohydrates_total_g { get; set; }
}
