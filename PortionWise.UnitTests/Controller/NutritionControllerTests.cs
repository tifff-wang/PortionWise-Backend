using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PortionWise.Controllers;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Nutrition.DTOs;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Services;
using PortionWise.UnitTests.MockData.Recipes;

namespace PortionWise.UnitTests.Controller
{
    public class NutritionControllerTests
    {
        private readonly NutritionController _nutritionController;
        private readonly Mock<INutritionService> _mockNutritionService;

        public NutritionControllerTests()
        {
            _mockNutritionService = new Mock<INutritionService>();
            _nutritionController = new NutritionController(_mockNutritionService.Object);
        }

        [Fact]
        public async void GetNutrition_NoException_ReturnOkwithTotalNutritionInfo()
        {
            var recipeId = Guid.NewGuid();
            var totalNutritionDTO = new TotalNutritionDTO
            {
                SugarGram = 10.5,
                FiberGram = 1.0,
                ServingSize = 375,
                SodiumMg = 200,
                PotassiumMg = 150,
                FatSaturatedGram = 2.5,
                FatTotalGram = 5,
                Calories = 180,
                CholesterolMg = 30,
                ProteinGram = 2.5,
                CarbohydratesTotalGram = 27,
            };
            _mockNutritionService
                .Setup(service => service.GetRecipeNutrition(recipeId))
                .ReturnsAsync(totalNutritionDTO);

            var response = await _nutritionController.GetNutrition(recipeId);
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.IsType<TotalNutritionDTO>(okResult.Value);
        }

        [Fact]
        public async void GetNutrition_NoRecipe_ReturnBadRequest()
        {
            var recipeId = Guid.NewGuid();
            _mockNutritionService
                .Setup(service => service.GetRecipeNutrition(recipeId))
                .ThrowsAsync(new RecipeNotFoundException());

            var response = await _nutritionController.GetNutrition(recipeId);
            var result = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async void GetNutrition_NoIngredients_ReturnNotFound()
        {
            var recipeId = Guid.NewGuid();
            _mockNutritionService
                .Setup(service => service.GetRecipeNutrition(recipeId))
                .ThrowsAsync(new IngredientNotFoundException());

            var response = await _nutritionController.GetNutrition(recipeId);
            var result = Assert.IsType<NotFoundObjectResult>(response.Result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void GetNutrition_UnexpectException_Return500()
        {
            var recipeId = Guid.NewGuid();
            _mockNutritionService
                .Setup(service => service.GetRecipeNutrition(recipeId))
                .ThrowsAsync(new Exception());

            var response = await _nutritionController.GetNutrition(recipeId);
            var result = Assert.IsType<ObjectResult>(response.Result);
            Assert.Equal(500, result.StatusCode);
        }
    }
}
