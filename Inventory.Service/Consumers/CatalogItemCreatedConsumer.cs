using Catalog.Contracts;
using Common.Kafka;
using Common.Repository;
using Inventory.Service.Models;

namespace Inventory.Service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> _catalogItemRepository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> catalogItemRepository) => _catalogItemRepository = catalogItemRepository;
        public string Topic => "ItemTopic";
        public int Partition => 0;

        public async Task ConsumeAsync(CatalogItemCreated? message)
        {
            var item = _catalogItemRepository.GetBy(message.ItemId);

            if (item == null)
            {
                item = new CatalogItem { Id = message.ItemId, Name = message.Name, Description = message.Description };
                await Task.FromResult(_catalogItemRepository.Create(item));
            }
        }
    }
}
