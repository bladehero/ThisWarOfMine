using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Common;

public static class CSharpFunctionalExtensions
{
    public static ValueTask<TValue> OnFallback<TValue, TError>(this Task<Result<TValue, TError>> taskResult,
        Func<TValue> fromError) => taskResult.OnFallback(_ => fromError());

    public static async ValueTask<TValue> OnFallback<TValue, TError>(this Task<Result<TValue, TError>> taskResult,
        Func<TError, TValue> fromError)
    {
        var result = await taskResult;
        return result.IsSuccess ? result.Value : fromError(result.Error);
    }
}