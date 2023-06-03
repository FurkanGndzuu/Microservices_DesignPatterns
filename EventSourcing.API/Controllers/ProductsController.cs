using EventSourcing.API.Commands;
using EventSourcing.API.DTOs;
using EventSourcing.API.Handlers;
using EventSourcing.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllListByUserId(int userId)
        {
            return Ok(await _mediator.Send(new GetProductsQuery() { userId = userId}));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO createProductDto)
        {
            await _mediator.Send(new CreateProductCommand() { CreateProduct = createProductDto });
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeName(ChangeProductNameDTO changeProductNameDto)
        {
            await _mediator.Send(new ChangeProductNameCommand { ChangeProductNameDto = changeProductNameDto });
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangePrice(ChangeProductPriceDTO changeProductPriceDto)
        {
            await _mediator.Send(new ChangeProductPriceCommand { changedProductPrice = changeProductPriceDto });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });
            return NoContent();
        }
    }
}
