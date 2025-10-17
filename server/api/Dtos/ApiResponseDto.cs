namespace api.Dtos
{
    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? UserInfo { get; set; }

        public ApiResponseDto(bool success, string message) { 
            Success = success;
            Message = message;
        }

        public ApiResponseDto(bool success, string message, object userInfo) {
            Success = success;
            Message = message;
            UserInfo = userInfo;
        }
    }
}
