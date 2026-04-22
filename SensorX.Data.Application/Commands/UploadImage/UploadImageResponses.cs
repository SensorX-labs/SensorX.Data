namespace SensorX.Data.Application.Commands.UploadImage;

/// <summary>
/// Represents a successful response for the image upload.
/// </summary>
public class UploadImageSuccessResponse
{
    public string FileUrl { get; }

    public UploadImageSuccessResponse(string fileUrl)
    {
        FileUrl = fileUrl;
    }
}

/// <summary>
/// Represents an error response for the image upload.
/// </summary>
public class UploadImageErrorResponse
{
    public string ErrorMessage { get; }

    public UploadImageErrorResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}

/// <summary>
/// Generic result class for returning either a success or an error response.
/// </summary>
public class Result<TSuccess, TError>
{
    public bool IsSuccess { get; }
    public TSuccess? Success { get; }
    public TError? Error { get; }

    private Result(bool isSuccess, TSuccess? success, TError? error)
    {
        IsSuccess = isSuccess;
        Success = success;
        Error = error;
    }

    public static Result<TSuccess, TError> SuccessResult(TSuccess success) =>
        new Result<TSuccess, TError>(true, success, default);

    public static Result<TSuccess, TError> ErrorResult(TError error) =>
        new Result<TSuccess, TError>(false, default, error);
}