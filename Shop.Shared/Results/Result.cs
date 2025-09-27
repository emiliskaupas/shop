namespace Shop.Shared.Results;

/// <summary>
/// Generic result type for operations that return data
/// </summary>
/// <typeparam name="T">The type of data returned on success</typeparam>
public class Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public List<string> Errors { get; init; } = new();

    /// <summary>
    /// Creates a successful result with data
    /// </summary>
    public static Result<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    /// <summary>
    /// Creates a failed result with a single error message
    /// </summary>
    public static Result<T> Failure(string error) => new()
    {
        IsSuccess = false,
        ErrorMessage = error,
        Errors = new List<string> { error }
    };

    /// <summary>
    /// Creates a failed result with multiple error messages
    /// </summary>
    public static Result<T> Failure(List<string> errors) => new()
    {
        IsSuccess = false,
        Errors = errors,
        ErrorMessage = string.Join(", ", errors)
    };

    /// <summary>
    /// Creates a failed result with exception details
    /// </summary>
    public static Result<T> Failure(Exception exception) => new()
    {
        IsSuccess = false,
        ErrorMessage = exception.Message,
        Errors = new List<string> { exception.Message }
    };
}

/// <summary>
/// Result type for operations that don't return data
/// </summary>
public class Result : Result<object>
{
    /// <summary>
    /// Creates a successful result without data
    /// </summary>
    public static Result Success() => new() 
    { 
        IsSuccess = true 
    };

    /// <summary>
    /// Creates a failed result with a single error message
    /// </summary>
    public static new Result Failure(string error) => new()
    {
        IsSuccess = false,
        ErrorMessage = error,
        Errors = new List<string> { error }
    };

    /// <summary>
    /// Creates a failed result with multiple error messages
    /// </summary>
    public static new Result Failure(List<string> errors) => new()
    {
        IsSuccess = false,
        Errors = errors,
        ErrorMessage = string.Join(", ", errors)
    };

    /// <summary>
    /// Creates a failed result with exception details
    /// </summary>
    public static new Result Failure(Exception exception) => new()
    {
        IsSuccess = false,
        ErrorMessage = exception.Message,
        Errors = new List<string> { exception.Message }
    };
}