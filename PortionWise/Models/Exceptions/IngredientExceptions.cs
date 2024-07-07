using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortionWise.Models.Exceptions
{
    public class IngredientNotFoundException : Exception
    {
        public string ErrorMessage = "Ingredient not found";
    }
}
