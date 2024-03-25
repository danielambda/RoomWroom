using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.DataGeneration;
using Api.IntegrationTests.Common.Extensions;
using Api.IntegrationTests.Common.Models;
using Contracts.Rooms;
using Domain.Common.Errors;
using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests.RoomController;

public class AddUserToRoomRoomControllerTests(IntegrationTestWebAppFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task AddUserToRoom_ShouldReturnOk_UserAndRoomExist()
    {
        //Arrange
        var room = Fakers.RoomFaker.Generate();
        var user = Fakers.UserFaker.Clone()
            .RuleFor(u => u.RoomId, _ => null)
            .Generate();

        DbContext.Rooms.Add(room);
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var request = new AddUserToRoomRequest(user.Id!);
        
        //Act
        var response = await Client.PostAsJsonAsync($"rooms/{room.Id.Value}/users", request);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var modifiedRoom = await DbContext.Rooms.FindAsync(room.Id);
        modifiedRoom!.UserIds.Should().Contain(user.Id);

        var userFromDb = await DbContext.Users.FindAsync(user.Id);
        userFromDb!.RoomId.Should().Be(room.Id);
    }
    
    [Fact]
    public async Task AddUserToRoom_ShouldReturnUserNotFound_UserDoesNotExist()
    {
        // Arrange
        var room = Fakers.RoomFaker.Generate();

        DbContext.Rooms.Add(room);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var request = new AddUserToRoomRequest(UserId.CreateNew()!);

        // Act
        var response = await Client.PostAsJsonAsync($"rooms/{room.Id.Value}/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.User.NotFound.Code);
    }
    
    [Fact]
    public async Task AddUserToRoom_ShouldReturnRoomNotFound_RoomDoesNotExist()
    {
        // Arrange
        var user = Fakers.UserFaker.Clone()
            .RuleFor(u => u.RoomId, _ => null)
            .Generate();

        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();
        
        var request = new AddUserToRoomRequest(user.Id!);

        // Act
        var response = await Client.PostAsJsonAsync($"rooms/{RoomId.CreateNew().Value}/users", request); 
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.Room.NotFound.Code);
    }
    
    [Fact]
    public async Task AddUserToRoom_ShouldReturnConflictAndErrorCode_UserAlreadyHasARoom()
    {
        // Arrange
        var usersInitialRoom = Fakers.RoomFaker.Generate();
        var user = Fakers.UserFaker.Clone()
            .RuleFor(u => u.RoomId, _ => usersInitialRoom.Id)
            .Generate(); 
        var room = Fakers.RoomFaker.Generate();

        await DbContext.Users.AddAsync(user);
        await DbContext.Rooms.AddRangeAsync(usersInitialRoom, room);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var request = new AddUserToRoomRequest(user.Id!);

        // Act
        var response = await Client.PostAsJsonAsync($"rooms/{room.Id.Value}/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.User.RoomAlreadySet(user.Id, user.RoomId!).Code);
    }
}