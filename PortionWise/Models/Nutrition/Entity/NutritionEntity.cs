using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Models.Nutrition.Entity
{
    public class NutritionEntity
    {
        [Key]
        public required Guid Id { get; set; }
        public double SugarGram { get; set; }
        public double FiberGram { get; set; }
        public double ServingSize { get; set; }
        public double SodiumMg { get; set; }
        public double PotassiumMg { get; set; }
        public double FatSaturatedGram { get; set; }
        public double FatTotalGram { get; set; }
        public double Calories { get; set; }
        public double CholesterolMg { get; set; }
        public double ProteinGram { get; set; }
        public double CarbohydratesTotalGram { get; set; }
        public DateTime? CacheExpirationTime { get; set; }

        public required Guid RecipeId { get; set; }
        public required RecipeEntity Recipe { get; set; }
    }
}
