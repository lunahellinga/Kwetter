namespace KwetterAPI.Models;

public class NewKweetDto
{
    public NewKweetDto(string message)
    {
        Message = message;
    }

    public string Message { get; set; }
}