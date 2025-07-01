using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyz.bll.Interfaces;
using xyz.bll;
using xyz.Data;
using xyz.Shared;

namespace xyz.Server
{
    public class OrderService : IOrderService
    {
        private readonly IOrderSvc _orderSvc;

        public OrderService()
        {
            // For now: manual DI — could be injected via container
            _orderSvc = new OrderSvc(new SqlDataServices());
        }

        public Order GetOrderById(int id)
            => _orderSvc.GetOrderByIdAsync(id).GetAwaiter().GetResult();

        public List<Order> GetAllOrders()
            => _orderSvc.GetAllOrdersAsync().GetAwaiter().GetResult();

        public void CreateOrder(Order order)
            => _orderSvc.CreateOrderAsync(order).GetAwaiter().GetResult();

        public void UpdateOrder(Order order)
            => _orderSvc.UpdateOrderAsync(order).GetAwaiter().GetResult();

        public bool DeleteOrder(int orderId)
            => _orderSvc.DeleteOrderAsync(orderId).GetAwaiter().GetResult();

        public List<Order> GetOrdersByCustomerId(int customerId)
            => _orderSvc.GetOrdersByCustomerIdAsync(customerId).GetAwaiter().GetResult();

        public List<Order> GetOrdersByStatus(string status)
            => _orderSvc.GetOrdersByStatusAsync(status).GetAwaiter().GetResult();

        public bool ChangeOrderStatus(int orderId, string newStatus)
            => _orderSvc.ChangeOrderStatusAsync(orderId, newStatus).GetAwaiter().GetResult();

        public decimal GetOrderTotal(int orderId)
            => _orderSvc.GetOrderTotalAsync(orderId).GetAwaiter().GetResult();

        public byte[] ExportOrderReport(int orderId)
            => _orderSvc.ExportOrderReportAsync(orderId).GetAwaiter().GetResult();
    }
}
