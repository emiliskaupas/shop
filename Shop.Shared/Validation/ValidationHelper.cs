using Shop.Shared.Results;

namespace Shop.Shared.Validation;

/// <summary>
/// Helper class for combining multiple validation results
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Combines multiple validation results into a single result
    /// </summary>
    public static Result<T> CombineValidationResults<T>(T value, params Result[] validationResults)
    {
        var errors = new List<string>();
        
        foreach (var result in validationResults)
        {
            if (!result.IsSuccess)
            {
                errors.AddRange(result.Errors);
            }
        }

        return errors.Count > 0 
            ? Result<T>.Failure(errors) 
            : Result<T>.Success(value);
    }

    /// <summary>
    /// Validates an object and returns detailed validation results
    /// </summary>
    public static Result ValidateObject<T>(T obj, params Func<T, Result>[] validators) where T : class
    {
        if (obj == null)
            return Result.Failure("Object cannot be null");

        var errors = new List<string>();

        foreach (var validator in validators)
        {
            var result = validator(obj);
            if (!result.IsSuccess)
            {
                errors.AddRange(result.Errors);
            }
        }

        return errors.Count > 0 
            ? Result.Failure(errors) 
            : Result.Success();
    }
}