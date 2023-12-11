using Catalog.Contracts;
using Catalog.Service.Models;
using Catalog.Service.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IItemService itemService, IPublishEndpoint publishEndpoint)
        {
            _itemService = itemService;
            _publishEndpoint = publishEndpoint;
        }
        // GET: api/<ItemsController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_itemService.Get());
        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = _itemService.GetById(id);
            return item != null ? Ok(item) : NotFound();
        }

        // POST api/<ItemsController>
        [HttpPost]
        public ActionResult<ItemDto> Post([FromBody] CreateItemDto value)
        {
            var item = _itemService.Create(value);

            if (item == null)
                return BadRequest();

            _publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemDto))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(Guid id, [FromBody] UpdateItemDto value)
        {
            var updated = _itemService.Update(id, value);

            if (!updated)
                return NotFound();

            _publishEndpoint.Publish(new CatalogItemUpdated(id, value.Name, value.Description));
            return Ok(value);
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var deleted = _itemService.Delete(id);

            if (!deleted)
                return NotFound();

            _publishEndpoint.Publish(new CatalogItemDeleted(id));
            return NoContent();
        }
    }
}
