namespace Contracts.Users;

public record CreateUserRequest(string Name, string UserRole, string? RoomId);