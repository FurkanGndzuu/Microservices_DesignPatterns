using EventSourcing.API.DTOs;
using EventSourcing.API.Models;
using EventSourcing.API.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.API.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDTO>>
    {
        private readonly AppDbContext _context;

        public GetProductsQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products.Where(x => x.UserId == request.userId).ToListAsync();

            return products.Select(x => new ProductDTO { Id = x.Id, Name = x.Name, Price = x.Price, Stock = x.Stock, UserId = x.UserId }).ToList();
        }
    }
}
