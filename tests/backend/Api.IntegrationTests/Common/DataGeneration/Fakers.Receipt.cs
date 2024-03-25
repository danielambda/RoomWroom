using Contracts.Receipts;
using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Api.IntegrationTests.Common.DataGeneration;

public static partial class Fakers
{
    public static Faker<Receipt> ReceiptFaker { get; } = new Faker<Receipt>()
        .CustomInstantiator(faker =>
        {
            var currency = faker.PickRandom<Currency>();

            return Receipt.CreateNew
            (
                faker.Make(faker.Random.Number(3, 10), () => new ReceiptItem
                (
                    faker.Commerce.Product(),
                    new Money
                    (
                        faker.Random.Decimal(),
                        currency
                    ),
                    faker.Random.Decimal(),
                    ShopItemId.CreateNew()
                )),
                faker.Random.String2(faker.Random.Number(5, 10)),
                UserId.CreateNew()
            );
        });
    
    public static Faker<CreateReceiptRequest> CreateReceiptRequestFaker { get; } = new Faker<CreateReceiptRequest>()
        .CustomInstantiator(faker =>
        {
            string currency = faker.PickRandom<Currency>().ToString();

            return new CreateReceiptRequest
            (
                faker.Make(faker.Random.Number(10), () => new ReceiptItemRequest
                (
                    faker.Commerce.Product(),
                    faker.Random.Decimal(),
                    currency,
                    faker.Random.Decimal(),
                    ShopItemId.CreateNew()!
                )),
                null
            );
        });
}