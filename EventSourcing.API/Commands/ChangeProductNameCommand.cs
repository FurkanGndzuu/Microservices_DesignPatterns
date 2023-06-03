using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Commands
{
    public class ChangeProductNameCommand : IRequest
    {
        public ChangeProductNameDTO ChangeProductNameDto { get; set; }
    }
}
