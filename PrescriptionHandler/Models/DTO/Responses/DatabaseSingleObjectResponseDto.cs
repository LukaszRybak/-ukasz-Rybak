namespace PrescriptionHandler.Models.DTO.Responses
{
    public class DatabaseSingleObjectResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Output { get; set; }

        public DatabaseSingleObjectResponseDto(int statusCode, string message, object output)
        {
            StatusCode = statusCode;
            Message = message;
            Output = output;
        }
    }
}
