namespace GameStore.API.Models;

public struct ErrorDetails
{
    public int StatusCode { get; set; }

    public string Message { get; set; }
}