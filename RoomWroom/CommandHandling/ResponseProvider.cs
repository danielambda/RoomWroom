namespace RoomWroom.CommandHandling;

public delegate Task<string>? ResponseProvider(string? text = null, Image? image = null);