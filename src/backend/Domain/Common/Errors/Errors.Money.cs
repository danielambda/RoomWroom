using ErrorOr;

namespace Domain.Common.Errors;

public static partial class Errors
{
    public static class Money
    {
        public static Error MismatchedCurrency =>
            Error.Validation("Money.MismatchedCurrency", "Currencies have to match to perform operations");
    }
}