using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.Web.Features.MyOrders;
using Microsoft.eShopWeb.Web.Features.OrderDetails;
using BlazorShared.Models;

namespace Microsoft.eShopWeb.Web.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrders([FromQuery] string userName)
    {
        Guard.Against.NullOrEmpty(userName, nameof(userName));
        var orders = await _mediator.Send(new GetMyOrders(userName));
        var result = orders.Select(o => new OrderDto
        {
            OrderNumber = o.OrderNumber,
            OrderDate = o.OrderDate,
            Total = o.Total,
            Status = o.Status,
            ShippingAddress = o.ShippingAddress == null ? null : new AddressDto
            {
                Street = o.ShippingAddress.Street,
                City = o.ShippingAddress.City,
                State = o.ShippingAddress.State,
                Country = o.ShippingAddress.Country,
                ZipCode = o.ShippingAddress.ZipCode
            }
        }).ToList();
        return Ok(result);
    }
}
