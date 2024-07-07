namespace PortionWise.Models.Exceptions
{
    public class RecipeMissingIdException : Exception
    {
        public string ErrorMessage = "recipe Id must be provided";
    }

    public class RecipeMissingNameException : Exception
    {
        public string ErrorMessage = "recipe name must be provided";
    }

    public class RecipeInvalidPortionSizeException : Exception
    {
        public string ErrorMessage = "portion size must be greater than 0";
    }

    public class RecipeNotFoundException : Exception
    {
        public string ErrorMessage = "Recipe not found";
    }
}
