using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Queries
{
    public class GetProductsQuery : IRequest<List<ProductDTO>>
    {
        public int userId { get; set; }
    }
}
