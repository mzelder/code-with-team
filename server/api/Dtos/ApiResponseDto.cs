namespace api.Dtos
{
    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ApiResponseDto(bool success, string message) { 
            Success = success;
            Message = message;
        }
    }
}
