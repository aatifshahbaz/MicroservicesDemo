using Catalog.Service.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
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
            return item != null ? CreatedAtAction(nameof(GetById), new { id = item.Id }, item) : BadRequest();
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemDto))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(Guid id, [FromBody] UpdateItemDto value)
        {
            return _itemService.Update(id, value) ? Ok(value) : NotFound();
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return _itemService.Delete(id) ? NoContent() : NotFound();
        }
    }
}
