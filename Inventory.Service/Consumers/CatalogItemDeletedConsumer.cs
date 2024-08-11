using Catalog.Contracts;
using Common.Kafka;
using Common.Repository;
using Inventory.Service.Models;

namespace Inventory.Service.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> _catalogItemRepository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> catalogItemRepository) => _catalogItemRepository = catalogItemRepository;
        public string Topic => "ItemTopic";
        public int Partition => 2;

        public async Task ConsumeAsync(CatalogItemDeleted? message)
        {
            var item = _catalogItemRepository.GetBy(message.ItemId);

            if (item != null)
                await Task.FromResult(_catalogItemRepository.Delete(item));
        }
    }
}
