global using Image = RoomWroom.Types.Image;

namespace RoomWroom.Types;

public class Image(byte[] byteArray)
{
    private byte[] ByteArray { get; } = byteArray;

    public static Image FromStream(Stream stream)
    {
        byte[] byteArray = [];
        _ = stream.Read(byteArray);
        return new(byteArray);
    }
    
    public static async Task<Image> FromStreamAsync(Stream stream)
    {
        byte[] byteArray = new byte[stream.Length];
        _ = await stream.ReadAsync(byteArray);
        return new(byteArray);
    }
    
    public static implicit operator byte[] (Image image) => image.ByteArray;
}