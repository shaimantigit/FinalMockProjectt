using AutoMapper;
using FinalMockProject.BLL;
using FinalMockProject.Models;
using FinalMockProject.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FinalMockProject.Controller
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private IMapper _mapper;

        public OrderController(OrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderByIdAsync(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> AddOrderAsync(OrderDTO orderDTO)
        {
            var addedOrder = await _orderService.AddOrderAsync(orderDTO);
            return CreatedAtAction(nameof(GetOrderByIdAsync), new { id = addedOrder.Order_Id }, addedOrder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDTO>> UpdateOrderAsync(int id, OrderDTO orderDTO)
        {
            if (id != orderDTO.Order_Id)
                return BadRequest();

            var updatedOrder = await _orderService.UpdateOrderAsync(orderDTO);
            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderAsync(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}



