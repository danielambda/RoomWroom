namespace RoomWroom.CommandHandling;

public interface IResponseProvider
{
    public Task<Response>? GetResponseTask(string? text, Image? image);
}