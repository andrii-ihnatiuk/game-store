namespace GameStore.Services.Models;

public class ErrorDetails
{
    public int Status { get; set; }

    public string Message { get; set; }

    public IList<string> Errors { get; set; }
}