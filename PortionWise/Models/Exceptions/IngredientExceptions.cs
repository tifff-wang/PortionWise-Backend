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

    public class IngredientMissingIdException : Exception
    {
        public string ErrorMessage = "Ingredient Id must be provided";
    }

    public class IngredientMissingNameException : Exception
    {
        public string ErrorMessage = "Please provide a ingredient name";
    }

    public class IngredientInvalidAmountException : Exception
    {
        public string ErrorMessage = "Ingredient amount must be larger than 0";
    }
}
