using Application.ShopItems.Commands;
using Application.ShopItems.Queries;
using Contracts.ShopItems;
using Domain.ShopItemAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Api.ShopItems;

[Route("shop-items")]
public class ShopItemsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateShopItemRequest request)
    {
        CreateShopItemCommand command = request.ToCommand();

        ErrorOr<ShopItem> result = await Mediator.Send(command);

        return result.Match(
            shopItem => OkCreated(shopItem.ToResponse()),
            errors => Problem(errors));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        GetShopItemQuery query = new(id!);

        ErrorOr<ShopItem> result = await Mediator.Send(query);

        return result.Match(
            shopItem => Ok(shopItem.ToResponse()),
            errors => Problem(errors));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        GetAllShopItemsQuery query = new();

        IQueryable<ShopItem> result = await Mediator.Send(query);

        return Ok(result.Select(item =>
            item.ToResponse()
        ));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        DeleteShopItemCommand command = new(id!);
        
        ErrorOr<Deleted> result = await Mediator.Send(command);

        return result.Match(
            _ => Ok(),
            errors => Problem(errors));
    }
}