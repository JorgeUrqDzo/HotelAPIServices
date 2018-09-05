namespace Hotel.WebApi.Responses
{
    public class ErrorResponse: ApiResponse
    {
        public ErrorResponse()
            : base("An unhandled error has occurred")
        {
        }

        public ErrorResponse(string errorMessage)
            : base(errorMessage)
        {

        }
    }
}