namespace Hotel.WebApi.Responses
{
    public class ApiResponse
    {
        public ApiResponse(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}