using Catalog.Contracts;
using Common.Kafka;
using Common.Repository;
using Inventory.Service.Models;

namespace Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> _catalogItemRepository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> catalogItemRepository) => _catalogItemRepository = catalogItemRepository;
        public string Topic => "ItemTopic";
        public int Partition => 1;

        public async Task ConsumeAsync(CatalogItemUpdated? message)
        {
            var item = _catalogItemRepository.GetBy(message.ItemId);

            if (item == null)
            {
                item = new CatalogItem { Id = message.ItemId, Name = message.Name, Description = message.Description };
                await Task.FromResult(_catalogItemRepository.Create(item));
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;
                await Task.FromResult(_catalogItemRepository.Update(item));
            }
        }
    }
}
