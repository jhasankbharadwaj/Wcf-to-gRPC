using System;
using System.Collections.Generic;

namespace Com.OldLeaf.Shared.Order
{
    public interface IOrderDirProxy
    {
        List<Order> SearchOrder(int id, OrderType orderType);
        Order GetOrder(int orderId);
        void SaveOrder(Order order);
        void DeleteOrder(int orderId);
    }
}