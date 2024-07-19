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
            CreateMap<IngredientDTO, UpdateIngredientBO>().ReverseMap();
            CreateMap<UpdateIngredientBO, IngredientEntity>().ReverseMap();
            CreateMap<IngredientBO, IngredientEntity>().ReverseMap();
            CreateMap<PopularIngredientsBO, PopularIngredientDTO>().ReverseMap();

            CreateMap<string, PopularIngredientsBO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src));

            CreateMap<CreateIngredientDTO, IngredientBO>()
                .ForMember(bo => bo.Id, opt => opt.MapFrom(dto => Guid.NewGuid()));
        }
    }
}
