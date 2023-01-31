namespace PrescriptionHandler.Models.DTO.Responses
{
    public class DatabaseCollectionObjectResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<object>? Output { get; set; }

        public DatabaseCollectionObjectResponseDto(int statusCode, string message, IEnumerable<object> output)
        {
            StatusCode = statusCode;
            Message = message;
            Output = output;
        }
    }
}
