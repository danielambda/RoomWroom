using Domain.ReceiptAggregate.ValueObjects;
using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.Enums;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UserAggregate;

public sealed class User : AggregateRoot<UserId>
{
    public string Name { get; private set; } = default!;
    public UserRole Role { get; private set; }
    public RoomId? RoomId { get; private set; }
    public IReadOnlyList<ReceiptId> ScannedReceiptsIds => _scannedReceiptsIds.AsReadOnly();

    private readonly List<ReceiptId> _scannedReceiptsIds = default!;

    private User(UserId id, string name, UserRole role, RoomId? roomId, List<ReceiptId> scannedReceiptIds) : base(id)
    {
        Name = name;
        RoomId = roomId;
        Role = role;

        _scannedReceiptsIds = scannedReceiptIds;
    }

    public static User CreateNew(string name, UserRole role, RoomId? roomId = null) =>
        new(UserId.CreateNew(), name, role, roomId, []);

    public static User Create(UserId userId, string name, UserRole role, RoomId? roomId, List<ReceiptId> receiptIds) =>
        new(userId, name, role, roomId, receiptIds);

    public void SetRoom(RoomId roomId) => RoomId = roomId;

    public void RemoveRoom() => RoomId = null;

    private User()
    {
    }
}