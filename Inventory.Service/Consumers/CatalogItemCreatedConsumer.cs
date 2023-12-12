using Catalog.Contracts;
using Common.Repository;
using Inventory.Service.Models;
using MassTransit;

namespace Inventory.Service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> _catalogItemRepository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> catalogItemRepository)
        {
            _catalogItemRepository = catalogItemRepository;
        }
        public Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;

            var item = _catalogItemRepository.GetBy(message.ItemId);

            if (item != null)
                return Task.CompletedTask;

            item = new CatalogItem { Id = message.ItemId, Name = message.Name, Description = message.Description };
            _catalogItemRepository.Create(item);
            return Task.CompletedTask;
        }
    }
}
