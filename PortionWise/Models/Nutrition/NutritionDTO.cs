using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortionWise.Models.Nutrition
{
    public class NutritionDTO
    {
        public required List<NutritionItem> Items { get; set; }
    }
}
