using Inventory.Service.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inventory.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvItemsController : ControllerBase
    {
        private readonly IInventoryItemService _invItemService;

        public InvItemsController(IInventoryItemService invItemService)
        {
            _invItemService = invItemService;
        }

        // GET: api/<InvItemsController>
        [HttpGet]
        public IActionResult Get(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest();

            var inventoryItems = _invItemService.Get(userId);
            return inventoryItems != null ? Ok(inventoryItems) : NotFound();
        }


        // POST api/<InvItemsController>
        [HttpPost]
        public IActionResult Post([FromBody] GrantItemDto dto)
        {
            return _invItemService.Create(dto) ? Ok() : BadRequest();
        }

    }
}
