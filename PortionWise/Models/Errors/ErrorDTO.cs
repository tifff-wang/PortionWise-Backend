namespace PortionWise.Models.Errors
{
    public class ErrorDTO
    {
        public string? Title { get; }
        public string? Detail { get; }

        public ErrorDTO(
            string? Title = null,
             string? Detail = null
             )
        {
            this.Title = Title;
            this.Detail = Detail;
        }

        public static ErrorDTO internalError()
        {
            return new ErrorDTO("Internal Server Error");
        }
    }

}
