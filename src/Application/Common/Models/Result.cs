using System;
using System.Collections.Generic;

namespace Application.Common.Models;

public class Result<T>
{
    private readonly T? _data;
    private readonly List<string> _errors;
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(T data)
    {
        IsSuccess = true;
        _data = data;
        _errors = new List<string>();
    }

    private Result(List<string> errors)
    {
        IsSuccess = false;
        _errors = errors ?? new List<string>();
    }

    public static Result<T> Success(T data) => new Result<T>(data);
    public static Result<T> Failure(List<string> errors) => new Result<T>(errors);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<List<string>, TResult> onFailure)
        => IsSuccess ? onSuccess(_data) : onFailure(_errors);
}
