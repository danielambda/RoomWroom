using Contracts.Rooms;
using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Api.IntegrationTests.Common.DataGeneration;

public static partial class Fakers
{
    public static Faker<Room> RoomFaker { get; } =
        new Faker<Room>()
            .CustomInstantiator(faker =>
            {
                var currency = faker.PickRandom<Currency>();

                return Room.CreateNew
                (
                    faker.Company.CompanyName(),
                    new Money
                    (
                        faker.Random.Decimal(),
                        currency
                    ),
                    faker.Random.Decimal(),
                    faker.Random.Bool(),
                    faker.Make(faker.Random.Number(10), () => UserId.CreateNew()),
                    faker.Make(faker.Random.Number(10), () => new OwnedShopItem
                    (
                        ShopItemId.CreateNew(),
                        faker.Random.Decimal(),
                        new Money
                        (
                            faker.Random.Decimal(),
                            currency
                        )
                    ))
                );
            });
    
    public static Faker<CreateRoomRequest> CreateRoomRequestFaker { get; } =
        new Faker<CreateRoomRequest>()
            .CustomInstantiator(faker =>
            {
                string currency = faker.PickRandom<Currency>().ToString();

                return new CreateRoomRequest
                (
                    faker.Company.CompanyName(),
                    faker.Random.Decimal(),
                    currency,
                    faker.Random.Decimal(),
                    faker.Random.Bool(),
                    faker.Make(faker.Random.Number(10), () => UserId.CreateNew().Value.ToString()),
                    faker.Make(faker.Random.Number(10), () =>
                        new OwnedShopItemRequest
                        (
                            ShopItemId.CreateNew()!, faker.Random.Decimal(),
                            faker.Random.Decimal(), currency
                        )
                    )
                );
            });
}