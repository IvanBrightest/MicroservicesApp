using MediatR;
using Ordering.Application.Mapper;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class GetOrderByUserNameHandler : IRequestHandler<GetOrderByUserNameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderRepository _repository;

        public GetOrderByUserNameHandler(IOrderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrderByUserNameQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _repository.GetOrdersByUserName(request.UserName);

            var orderResponseList = OrderMapper.Mapper.Map<IEnumerable<OrderResponse>>(orderList);

            return orderResponseList;
        }
    }
}
