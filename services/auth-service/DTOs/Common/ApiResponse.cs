// DTOs/Common/ApiResponse.cs
namespace AuthService.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public object? Errors { get; set; }
        public object? Meta { get; set; }

        public ApiResponse() { }

        public ApiResponse(T? data, bool success = true, string? message = null, object? errors = null, object? meta = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
            Meta = meta;
        }

        public static ApiResponse<T> SuccessResponse(T? data, string? message = null, object? meta = null)
        {
            return new ApiResponse<T>(data, true, message, null, meta);
        }

        public static ApiResponse<T> Fail(string message, object? errors = null)
        {
            return new ApiResponse<T>(default, false, message, errors);
        }
    }
}