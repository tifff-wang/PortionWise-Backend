namespace PortionWise.Models.Exceptions
{
    public class RecipeMissingNameException : Exception {
        public string ErrorMessage = "recipe name must be provided";
    }

    public class RecipeInvalidPortionSizeException : Exception {
        public string ErrorMessage = "portion size must be greater than 0";
    }
}
