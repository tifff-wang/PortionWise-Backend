using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortionWise.Models.Exceptions
{
    public class NutritionInfoNotFoundException : Exception
    {
        public string ErrorMessage = "no nutrition info found";
    }
}