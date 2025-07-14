using System;
using System.Collections.Generic;
using Com.OldLeaf.Shared.Order;
using Com.OldLeaf.Shared.Person;
using Com.OldLeaf.Shared.Delivery;

namespace Com.OldLeaf.Shared.Customer
{
    public interface ICustomerDirProxy
    {
        Customer GetCustomer(int customerId);
        List<Order.Order> SearchOrders(int customerId, string criteria);
        Person.Person SavePerson(Person.Person person);
        Delivery.Delivery GetDeliveryDate(int orderId, string name);
    }
}
