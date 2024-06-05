using AutoMapper;
using PortionWise.Models.Recipe.BO;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Models.Recipe
{
    public class RecipeMapping : Profile
    {
        public RecipeMapping()
        {
            CreateMap<RecipeSummaryDTO, RecipeBO>().ReverseMap();

            CreateMap<RecipeDTO, RecipeBO>().ReverseMap();
            CreateMap<RecipeBO, RecipeEntity>().ReverseMap();

            CreateMap<CreateRecipeDTO, RecipeBO>()
                .ForMember(bo => bo.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(bo => bo.CreatedAt, opt => opt.MapFrom(dto => DateTime.UtcNow));
        }
    }
}
