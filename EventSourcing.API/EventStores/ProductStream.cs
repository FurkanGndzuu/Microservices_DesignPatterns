using EventSourcing.API.DTOs;
using EventStore.ClientAPI;
using Shared.Events;

namespace EventSourcing.API.EventStores
{
    public class ProductStream : AbstractStream
    {
        public static string streamName => "ProductStream";
        public static string GroupName => "agroup";
        protected ProductStream( IEventStoreConnection eventStoreConnection) : base(streamName, eventStoreConnection)
        {
        }
        public void Created(CreateProductDTO createProductDto)
        {
            Events.AddLast(new ProductCreatedEvent { Id = Guid.NewGuid(), Name = createProductDto.Name, Price = createProductDto.Price, Stock = createProductDto.Stock, UserId = createProductDto.UserId });
        }

        public void NameChanged(ChangeProductNameDTO changeProductNameDto)
        {
            Events.AddLast(new ProductNameChangedEvent { ProductNewName = changeProductNameDto.Name, Id = changeProductNameDto.Id });
        }

        public void PriceChanged(ChangeProductPriceDTO changeProductPriceDto)
        {
            Events.AddLast(new ProductPriceChangedEvent() { ProductNewPrice = changeProductPriceDto.Price, Id = changeProductPriceDto.Id });
        }

        public void Deleted(Guid id)
        {
            Events.AddLast(new ProductDeletedEvent { Id = id });
        }
    }
}
