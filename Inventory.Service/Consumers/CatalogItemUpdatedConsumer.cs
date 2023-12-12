using Catalog.Contracts;
using Common.Repository;
using Inventory.Service.Models;
using MassTransit;

namespace Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> _catalogItemRepository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> catalogItemRepository)
        {
            _catalogItemRepository = catalogItemRepository;
        }
        public Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;

            var item = _catalogItemRepository.GetBy(message.ItemId);

            if (item == null)
            {
                item = new CatalogItem { Id = message.ItemId, Name = message.Name, Description = message.Description };
                _catalogItemRepository.Create(item);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;
                _catalogItemRepository.Update(item);
            }

            return Task.CompletedTask;
        }
    }
}
