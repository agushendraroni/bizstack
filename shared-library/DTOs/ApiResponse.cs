namespace SharedLibrary.DTOs;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public object? Errors { get; set; }
    public object? Meta { get; set; }

    public ApiResponse() { }

    public ApiResponse(T? data, bool isSuccess = true, string? message = null, object? errors = null, object? meta = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
        Errors = errors;
        Meta = meta;
    }

    public static ApiResponse<T> Success(T? data, string? message = null, object? meta = null)
    {
        return new ApiResponse<T>(data, true, message, null, meta);
    }

    public static ApiResponse<T> Error(string message, object? errors = null)
    {
        return new ApiResponse<T>(default, false, message, errors);
    }
}
