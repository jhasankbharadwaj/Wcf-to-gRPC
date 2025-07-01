using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyz.Data;

namespace xyz.bll.Interfaces
{
    public interface IOrderSvc
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<List<Order>> GetOrdersByDateRangeAsync(DateTime from, DateTime to);
        Task<List<Order>> GetOrdersByStatusAsync(string status);

        Task<int> CreateOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<decimal> CalculateOrderTotalAsync(int orderId);

        Task<byte[]> GenerateOrderInvoiceAsync(int orderId);
    }
}
