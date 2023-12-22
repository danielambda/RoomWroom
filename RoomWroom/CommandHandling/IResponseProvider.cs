namespace RoomWroom.CommandHandling;

public interface IResponseProvider
{
    public string? GetResponse(string message);
}