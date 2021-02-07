using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrdersByUserName(string userName)
        {
            GetOrderByUserNameQuery query = new GetOrderByUserNameQuery(userName);
            IEnumerable<OrderResponse> orders = await _mediator.Send(query);
            return Ok(orders);

        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckoutOrder([FromBody] CheckoutOrderCommand command) => Ok(await _mediator.Send(command));
    }
}
