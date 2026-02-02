namespace Application.Common.Models;

public class Result<T>
{
    private readonly T? _data;
    private readonly List<string> _errors;

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

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result<T> Success(T data)
    {
        return new Result<T>(data);
    }

    public static Result<T> Failure(List<string> errors)
    {
        return new Result<T>(errors);
    }

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<List<string>, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(_data!) : onFailure(_errors);
    }
}
