using System.Collections.Concurrent;
using System.Text.Json;
using Application.Common.Interfaces.Perception;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.Enums;
using Domain.UserAggregate.ValueObjects;

namespace Infrastructure.Users.Perception;

public class FileUserRepository : IUserRepository
{
    private const string USERS_FILE = "Users.json";
    
    private static readonly ConcurrentDictionary<UserId, User> Users = InitUsers();

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        Users.TryAdd(user.Id, user);
        
        UpdateUsersFile();
        return Task.CompletedTask;
    }

    public Task<User?> GetAsync(UserId id, CancellationToken cancellationToken) =>
        Task.FromResult(Users.GetValueOrDefault(id));


    public Task<bool> CheckExistenceAsync(UserId id, CancellationToken cancellationToken) =>
        Task.FromResult(Users.ContainsKey(id));

    public Task SaveChangesAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static ConcurrentDictionary<UserId, User> InitUsers()
    {
        using FileStream stream = new(USERS_FILE, FileMode.OpenOrCreate, FileAccess.Read);
        Span<byte> buffer = stackalloc byte[(int)stream.Length];
        _ = stream.Read(buffer);

        if (buffer.Length == 0)
            return [];

        Utf8JsonReader jsonReader = new(buffer);
        JsonElement jsonElement = JsonElement.ParseValue(ref jsonReader);

        ConcurrentDictionary<UserId, User>? users = jsonElement.Deserialize();
        
        return users ?? [];
    }
    
    private static void UpdateUsersFile()
    {
        using FileStream stream = new(USERS_FILE, FileMode.OpenOrCreate, FileAccess.Write);
        string json = Users.Serialize();
        Span<byte> buffer = System.Text.Encoding.UTF8.GetBytes(json);

        stream.Write(buffer);
    }
}

file static class SerializationExtensions
{
    public static ConcurrentDictionary<UserId, User>? Deserialize(this JsonElement jsonElement)
    {
        if(jsonElement.Deserialize<Dictionary<string, UserDto>>() is not { } userDtos)
            return null;

        return new(userDtos.Select(pair => 
            new KeyValuePair<UserId, User>(
                pair.Key!,
                User.Create(
                    pair.Value.Id!,
                    pair.Value.Name,
                    Enum.Parse<UserRole>(pair.Value.Role),
                    pair.Value.RoomId!,
                    pair.Value.ScannedReceiptsIds.ConvertAll(id =>
                        ReceiptId.Create(Guid.Parse(id))
                    )
                )
            )
        ).ToDictionary());
    }

    public static string Serialize(this ConcurrentDictionary<UserId, User> users) =>
        JsonSerializer.Serialize(users.Select(pair => 
            new KeyValuePair<string, UserDto>(
                pair.Key!,
                new UserDto(
                    pair.Value.Id!,
                    pair.Value.Name,
                    pair.Value.Role.ToString(),
                    pair.Value.RoomId!,
                    pair.Value.ScannedReceiptsIds.Select(id => id.Value.ToString()).ToList()
                )
            )).ToDictionary()
        );

    private record UserDto(string Id, string Name, string Role, string RoomId, List<string> ScannedReceiptsIds);
}