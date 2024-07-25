using AutoMapper;
using PortionWise.Models.Nutrition;
using PortionWise.Models.Nutrition.BOs;
using PortionWise.Models.Nutrition.DLs;
using PortionWise.Models.Nutrition.DTOs;
using PortionWise.Models.Nutrition.Entity;

namespace PortionWise.Models.Nutrition
{
    public class NutritionMapping : Profile
    {
        public NutritionMapping()
        {
            CreateMap<TotalNutritionDTO, TotalNutritionBO>().ReverseMap();
            CreateMap<TotalNutritionBO, TotalNutritionDL>().ReverseMap();
            CreateMap<NutritionEntity, TotalNutritionBO>().ReverseMap();

            CreateMap<TotalNutritionDL, NutritionEntity>()
                .ForMember(
                    entity => entity.CacheExpirationTime,
                    opt => opt.MapFrom(dl => DateTime.UtcNow.AddMinutes(5))
                )
                .ForMember(entity => entity.Id, opt => opt.MapFrom(dl => Guid.NewGuid()));
        }
    }
}
