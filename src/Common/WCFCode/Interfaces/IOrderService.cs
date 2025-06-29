namespace ECommerce.Interfaces
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        List<Order> GetAllOrders();
        
        [OperationContract]
        Order GetOrderById(int orderId);
        
        [OperationContract]
        List<Order> GetOrdersByCustomer(int customerId);
        
        [OperationContract]
        int CreateOrder(Order order);
        
        [OperationContract]
        bool UpdateOrderStatus(int orderId, string status);
        
        [OperationContract]
        bool CancelOrder(int orderId);
    }
}