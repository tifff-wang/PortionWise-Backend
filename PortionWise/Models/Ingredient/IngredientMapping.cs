using PortionWise.Models.Ingredient.BOs;
using PortionWise.Models.Ingredient.DTOs;
using PortionWise.Models.Ingredient.Entities;
using Profile = AutoMapper.Profile;

namespace PortionWise.Models.Ingredient
{
    public class IngredientMapping : Profile
    {
        public IngredientMapping()
        {
            CreateMap<IngredientDTO, IngredientBO>().ReverseMap();
            CreateMap<IngredientBO, IngredientEntity>().ReverseMap();

            CreateMap<CreateIngredientDTO, IngredientBO>()
                .ForMember(bo => bo.Id, opt => opt.MapFrom(dto => Guid.NewGuid()));
        }
    }
}
