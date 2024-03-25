using Contracts.ShopItems;
using Domain.Common.Enums;
using Domain.IngredientsAggregate.ValueObjects;
using Domain.ShopItemAggregate;

namespace Api.IntegrationTests.Common.DataGeneration;

public static partial class Fakers
{
    public static Faker<ShopItem> ShopItemFaker { get; } = new Faker<ShopItem>()
        .CustomInstantiator(faker => ShopItem.CreateNew
        (
            faker.Commerce.Product(),
            faker.Random.Decimal(),
            faker.PickRandom<Units>(),
            IngredientId.CreateNew()
        ));

    public static Faker<CreateShopItemRequest> CreateShopItemRequestFaker { get; } = new Faker<CreateShopItemRequest>()
        .CustomInstantiator(faker => new
        (
            faker.Commerce.Product(),
            faker.Random.Decimal(),
            faker.PickRandom<Units>().ToString(),
            IngredientId.CreateNew()
        ));
}