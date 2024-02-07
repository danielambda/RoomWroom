namespace Domain.Common.Models;

public interface IId<TSelf, TSource> 
    where TSelf : IId<TSelf, TSource>
    where TSource : notnull
{
    public TSource Value { get; }

    public static abstract TSelf Create(TSource value);
    public static abstract TSelf CreateUnique();
    
    public static abstract implicit operator string?(TSelf? id);
    public static abstract implicit operator TSelf?(string? str);

    public int GetHashCode() => Value.GetHashCode();
    public string? ToString() => Value.ToString();
}