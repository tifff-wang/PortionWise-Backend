using AutoMapper;
using PortionWise.Models.Nutrition;
using PortionWise.Models.Nutrition.BOs;
using PortionWise.Models.Nutrition.DLs;
using PortionWise.Models.Nutrition.DTOs;

namespace PortionWise.Models.Nutrition
{
  public class NutritionMapping : Profile
    {
        public NutritionMapping()
        {
            CreateMap<TotalNutritionDTO, TotalNutritionBO>().ReverseMap();
            CreateMap<TotalNutritionBO, TotalNutritionDL>().ReverseMap();
        }
    }
}
