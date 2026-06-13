namespace Application.Dtos;

/// <summary>
/// Standard error response DTO for API error handling.
/// Provides a consistent error message format across all endpoints.
/// </summary>
public class Error
{
    /// <summary>
    /// The error message describing what went wrong.
    /// </summary>
    public string error { get; set; }

    /// <summary>
    /// Initializes a new error with the specified message.
    /// </summary>
    /// <param name="errorMessage">The error message to include in the response.</param>
    public Error(string errorMessage)
    {
        error = errorMessage;
    }

    /// <summary>
    /// Initializes a new error with an empty message.
    /// </summary>
    public Error() : this(string.Empty) { }
}