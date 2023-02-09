namespace Tutorial_DotNet.Models;

public class ServiceResponse<T> {
    public ServiceResponse(T? data, bool success, string message) {
        Data = data;
        Success = success;
        Message = message;
    }
    public ServiceResponse() { }
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = String.Empty;
}