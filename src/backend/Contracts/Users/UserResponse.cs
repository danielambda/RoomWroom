namespace Contracts.Users;

public record UserResponse(string Id, string Name, string UserRole, string? RoomId);