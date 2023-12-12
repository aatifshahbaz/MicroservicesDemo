using Catalog.Contracts;
using Common.Repository;
using Inventory.Service.Models;
using MassTransit;

namespace Inventory.Service.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> _catalogItemRepository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> catalogItemRepository)
        {
            _catalogItemRepository = catalogItemRepository;
        }
        public Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;

            var item = _catalogItemRepository.GetBy(message.ItemId);

            if (item != null)
                _catalogItemRepository.Delete(item);

            return Task.CompletedTask;
        }
    }
}
