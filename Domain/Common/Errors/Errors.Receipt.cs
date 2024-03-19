using Domain.ReceiptAggregate.ValueObjects;
using ErrorOr;

namespace Domain.Common.Errors;

public static partial class Errors
{
    public static class Receipt
    {
        public static Error NotFound(ReceiptId id) =>
            Error.NotFound($"Receipt.{nameof(NotFound)}", $"Receipt with id '{id.Value}' was not found");

        public static Error QrDuplicate(string qr) =>
            Error.Conflict($"Receipt.{nameof(QrDuplicate)}", $"Receipt with qr {qr} already exists");

        public static Error FromQrCreationFailure(string qr) =>
            Error.Failure($"Receipt.{nameof(FromQrCreationFailure)}", $"Receipt with qr {qr} was failed to create");
    }
}