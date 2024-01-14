namespace Domain.Common.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; }
    public Currency Currency { get; }
    
    public Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    public override string ToString() => $"{Amount} {Currency}";

    public static Money operator *(Money left, float right) => new(left.Amount * (decimal)right, left.Currency);
    public static Money operator *(float left, Money right) => new(right.Amount * (decimal)left, right.Currency);

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new MismatchedСurrenciesExceptions();

        return new(left.Amount + right.Amount, left.Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
