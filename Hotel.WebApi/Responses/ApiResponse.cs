namespace Hotel.WebApi.Responses
{
    public class ApiResponse
    {
        public string Message { get; }

        public ApiResponse(string message)
        {
            Message = message;
        }
    }
}