using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.Enums;

namespace Api.IntegrationTests.Common.DataGeneration;

public static partial class Fakers
{
    public static Faker<User> UserFaker { get; } = new Faker<User>()
        .CustomInstantiator(faker => User.CreateNew
        (
            faker.Name.FullName(),
            faker.PickRandom<UserRole>(),
            RoomId.CreateNew()
        ));
}