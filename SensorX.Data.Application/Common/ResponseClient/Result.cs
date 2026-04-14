namespace SensorX.Data.Application.Common.ResponseClient
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }

        protected Result(bool isSuccess, T? value, string? error)
        {
            if (isSuccess && error != null)
                throw new InvalidOperationException("Success result cannot have error");
            
            // Check if value is not default (works for both reference and value types)
            if (!isSuccess && value != null && !value.Equals(default(T)))
                throw new InvalidOperationException("Failure result should not have meaningful value");

            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(string error) => new(false, default, error);
        public static implicit operator bool(Result<T>? result) => result is not null && result.IsSuccess;
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }

        protected Result(bool isSuccess, string? error)
        {
            if (isSuccess && error != null)
                throw new InvalidOperationException("Success result cannot have error");
            if (!isSuccess && error == null)
                throw new InvalidOperationException("Failure result must have error message");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(string error) => new(false, error);
    }
}