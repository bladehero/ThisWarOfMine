﻿using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Common;

public static class CSharpFunctionalExtensions
{
    public static TValue OnFallback<TValue, TError>(
        this Result<TValue, TError> result,
        Func<TError, TValue> fromError
    ) => result.IsSuccess ? result.Value : fromError(result.Error);

    public static TValue OnFallback<TValue>(this Result<TValue> result, Func<string, TValue> fromError) =>
        result.IsSuccess ? result.Value : fromError(result.Error);
}
