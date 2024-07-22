using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortionWise.Models.Recipe.BOs
{
    public class UpdateRecipeBO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public int portionSize { get; set; }
        public required string Instruction { get; set; }
    }
}
