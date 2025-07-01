using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace xyz.Shared
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        Order GetOrderById(int id);

        [OperationContract]
        List<Order> GetAllOrders();

        [OperationContract]
        void CreateOrder(Order order);

        [OperationContract]
        void UpdateOrder(Order order);

        [OperationContract]
        bool DeleteOrder(int orderId);

        [OperationContract]
        List<Order> GetOrdersByCustomerId(int customerId);

        [OperationContract]
        List<Order> GetOrdersByStatus(string status);

        [OperationContract]
        bool ChangeOrderStatus(int orderId, string newStatus);

        [OperationContract]
        decimal GetOrderTotal(int orderId);

        [OperationContract]
        byte[] ExportOrderReport(int orderId); // Assume it returns a PDF as byte[]
    }
