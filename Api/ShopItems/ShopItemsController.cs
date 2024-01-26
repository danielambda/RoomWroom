using Application.ShopItems.Commands;
using Application.ShopItems.Queries;
using Contracts.ShopItems;
using Domain.ShopItemAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Api.ShopItems;

[Route("shop")]
public class ShopItemsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(GetShopItemRequest request, string id)
    {
        GetShopItemQuery query = (request, id).ToQuery();

        ErrorOr<ShopItem> result = await _mediator.Send(query);

        return result.Match(
            shopItem => Ok(shopItem.ToResponse()),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateShopItemRequest request)
    {
        CreateShopItemCommand command = request.ToCommand();

        ErrorOr<ShopItem> result = await _mediator.Send(command);

        return result.Match(
            shopItem => OkCreated(shopItem.ToResponse()),
            errors => Problem(errors));
    }
}