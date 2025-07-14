using System;
using System.Collections.Generic;
using Com.OldLeaf.Shared.Order;
using OldLeaf.Services;

namespace OldLeaf.OldKey.BLL.Order
{
    public class OrderDirProxyImpl : IOrderDirProxy
    {
        private readonly SqlDataServices _dataServices;

        public OrderDirProxyImpl()
        {
            _dataServices = new SqlDataServices();
        }

        public OrderDirProxyImpl(string connectionString)
        {
            _dataServices = new SqlDataServices(connectionString);
        }

        public List<Com.OldLeaf.Shared.Order.Order> SearchOrder(int id, OrderType orderType)
        {
            string criteria = $"CustomerId = {id} AND OrderType = {(int)orderType}";
            return _dataServices.Search<Com.OldLeaf.Shared.Order.Order>(criteria);
        }

        public Com.OldLeaf.Shared.Order.Order GetOrder(int orderId)
        {
            return _dataServices.Get<Com.OldLeaf.Shared.Order.Order>(orderId);
        }

        public void SaveOrder(Com.OldLeaf.Shared.Order.Order order)
        {
            _dataServices.Save(order);
        }

        public void DeleteOrder(int orderId)
        {
            _dataServices.Remove<Com.OldLeaf.Shared.Order.Order>(orderId);
        }
    }
}
