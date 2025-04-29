namespace GoodExpense.Common.Domain;

public record ApiResponse
{
    public string? Message { get; set; }
}

public record ApiResult<T> : ApiResponse
{
    public T? Data { get; set; }
    
    public ApiResult() { }

    public ApiResult(T? data)
    {
        Data = data;
    }
    
    public ApiResult(T? data, string? message) : this(data)
    {
        Message = message;
    }
}