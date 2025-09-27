using Shop.Shared.Results;

namespace Shop.Shared.Validation;

/// <summary>
/// Extension methods for common validation scenarios
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Validates that a reference type is not null
    /// </summary>
    public static Result<T> ValidateRequired<T>(this T? value, string fieldName) where T : class
    {
        if (value == null)
            return Result<T>.Failure($"{fieldName} is required");
        return Result<T>.Success(value);
    }

    /// <summary>
    /// Validates that a nullable value type has a value
    /// </summary>
    public static Result<T> ValidateRequired<T>(this T? value, string fieldName) where T : struct
    {
        if (!value.HasValue)
            return Result<T>.Failure($"{fieldName} is required");
        return Result<T>.Success(value.Value);
    }

    /// <summary>
    /// Validates that a string is not null or empty
    /// </summary>
    public static Result<string> ValidateNotEmpty(this string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<string>.Failure($"{fieldName} cannot be empty");
        return Result<string>.Success(value);
    }

    /// <summary>
    /// Validates that a decimal value is positive
    /// </summary>
    public static Result<decimal> ValidatePositive(this decimal value, string fieldName)
    {
        if (value <= 0)
            return Result<decimal>.Failure($"{fieldName} must be greater than 0");
        return Result<decimal>.Success(value);
    }

    /// <summary>
    /// Validates that an integer value is positive
    /// </summary>
    public static Result<int> ValidatePositive(this int value, string fieldName)
    {
        if (value <= 0)
            return Result<int>.Failure($"{fieldName} must be greater than 0");
        return Result<int>.Success(value);
    }

    /// <summary>
    /// Validates that a decimal value is non-negative
    /// </summary>
    public static Result<decimal> ValidateNonNegative(this decimal value, string fieldName)
    {
        if (value < 0)
            return Result<decimal>.Failure($"{fieldName} cannot be negative");
        return Result<decimal>.Success(value);
    }

    /// <summary>
    /// Validates that an integer value is non-negative
    /// </summary>
    public static Result<int> ValidateNonNegative(this int value, string fieldName)
    {
        if (value < 0)
            return Result<int>.Failure($"{fieldName} cannot be negative");
        return Result<int>.Success(value);
    }

    /// <summary>
    /// Validates that a string meets minimum length requirements
    /// </summary>
    public static Result<string> ValidateMinLength(this string? value, int minLength, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<string>.Failure($"{fieldName} is required");

        if (value.Length < minLength)
            return Result<string>.Failure($"{fieldName} must be at least {minLength} characters long");

        return Result<string>.Success(value);
    }

    /// <summary>
    /// Validates that a string doesn't exceed maximum length
    /// </summary>
    public static Result<string> ValidateMaxLength(this string? value, int maxLength, string fieldName)
    {
        if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
            return Result<string>.Failure($"{fieldName} cannot exceed {maxLength} characters");

        return Result<string>.Success(value ?? string.Empty);
    }

    /// <summary>
    /// Validates that a value is within a specified range
    /// </summary>
    public static Result<T> ValidateRange<T>(this T value, T min, T max, string fieldName)
        where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            return Result<T>.Failure($"{fieldName} must be between {min} and {max}");

        return Result<T>.Success(value);
    }
    
    /// <summary>
    /// Validates that a string is a valid email format
    /// </summary>
    public static Result<string> ValidateEmail(this string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<string>.Failure($"{fieldName} is required");

        if (!value.Contains('@') || !value.Contains('.'))
            return Result<string>.Failure($"{fieldName} must be a valid email address");

        return Result<string>.Success(value);
    }
}