namespace Infrastructure.Shopping.Dtos;

public record ReceiptItemDto(string Name, int Price, float Quantity, int Sum);