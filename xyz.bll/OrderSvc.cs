using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyz.bll.Interfaces;
using xyz.Data;

namespace xyz.bll
{
    public class OrderSvc : IOrderSvc
    {
        private readonly SqlDataServices _sqlDataServices;

        public OrderSvc(SqlDataServices sqlDataServices)
        {
            _sqlDataServices = sqlDataServices;
        }

        public Task<Order> GetOrderByIdAsync(int id)
            => _sqlDataServices.GetOrderByIdAsync(id);

        public Task<List<Order>> GetAllOrdersAsync()
            => _sqlDataServices.GetAllOrdersAsync();

        public Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
            => _sqlDataServices.GetOrdersByCustomerIdAsync(customerId);

        public Task<List<Order>> GetOrdersByDateRangeAsync(DateTime from, DateTime to)
            => _sqlDataServices.GetOrdersByDateRangeAsync(from, to);

        public Task<List<Order>> GetOrdersByStatusAsync(string status)
            => _sqlDataServices.GetOrdersByStatusAsync(status);

        public Task<int> CreateOrderAsync(Order order)
            => _sqlDataServices.CreateOrderAsync(order);

        public Task<bool> UpdateOrderAsync(Order order)
            => _sqlDataServices.UpdateOrderAsync(order);

        public Task<bool> DeleteOrderAsync(int orderId)
            => _sqlDataServices.DeleteOrderAsync(orderId);

        public Task<decimal> CalculateOrderTotalAsync(int orderId)
            => _sqlDataServices.CalculateOrderTotalAsync(orderId);

        public Task<byte[]> GenerateOrderInvoiceAsync(int orderId)
            => _sqlDataServices.GenerateOrderInvoiceAsync(orderId);
    }
}
