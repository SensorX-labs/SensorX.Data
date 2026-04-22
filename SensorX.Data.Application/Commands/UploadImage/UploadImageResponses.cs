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
