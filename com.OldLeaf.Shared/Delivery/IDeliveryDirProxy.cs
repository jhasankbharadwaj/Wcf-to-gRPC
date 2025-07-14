using System;
using Com.OldLeaf.Shared.Customer;

namespace Com.OldLeaf.Shared.Delivery
{
    public interface IDeliveryDirProxy
    {
        Delivery GetDeliveryByOrder(int orderId, ICustomerDirProxy customerProxy);
    }
}