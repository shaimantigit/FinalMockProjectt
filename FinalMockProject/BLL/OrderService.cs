using FinalMockProject.DAL.Interface;
using FinalMockProject.Models.DTO;
using FinalMockProject.Models;
using FinalMockProject.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FinalMockProject.DAL;
using Microsoft.EntityFrameworkCore;


namespace FinalMockProject.BLL
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;
        public OrderService(ApplicationDbContext context,IOrderRepository orderRepository,IMapper mapper)
        {
            _context = context;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> AddOrderAsync(OrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO);
            var addedOrder = await _orderRepository.AddOrderAsync(order);
            return _mapper.Map<OrderDTO>(addedOrder);
        }

        public async Task<OrderDTO> UpdateOrderAsync(OrderDTO orderDTO)
        {
            var order = _mapper.Map<Order>(orderDTO);
            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);
            return _mapper.Map<OrderDTO>(updatedOrder);
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            return await _orderRepository.DeleteOrderAsync(orderId);
        }
    }
}

