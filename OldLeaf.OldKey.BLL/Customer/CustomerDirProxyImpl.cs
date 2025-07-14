using System;
using System.Collections.Generic;
using Com.OldLeaf.Shared.Customer;
using Com.OldLeaf.Shared.Person;
using Com.OldLeaf.Shared.Order;
using Com.OldLeaf.Shared.Delivery;
using OldLeaf.Services;

namespace OldLeaf.OldKey.BLL.Customer
{
    public class CustomerDirProxyImpl : ICustomerDirProxy
    {
        private readonly IPersonDirProxy _personProxy;
        private readonly IOrderDirProxy _orderProxy;
        private readonly IDeliveryDirProxy _deliveryProxy;

        public CustomerDirProxyImpl(IPersonDirProxy personProxy, IOrderDirProxy orderProxy, IDeliveryDirProxy deliveryProxy)
        {
            _personProxy = personProxy;
            _orderProxy = orderProxy;
            _deliveryProxy = deliveryProxy;
        }

        public Com.OldLeaf.Shared.Customer.Customer GetCustomer(int customerId)
        {
            // Call person proxy to get person details
            var person = _personProxy.Get(customerId);

            // Call order proxy to get customer orders
            var orders = _orderProxy.SearchOrder(customerId, OrderType.Online);

            // Call delivery proxy for delivery information
            var delivery = _deliveryProxy.GetDeliveryByOrder(customerId, this);

            // Create and return customer object
            return new Com.OldLeaf.Shared.Customer.Customer
            {
                CustomerId = customerId,
                FirstName = person?.FirstName,
                LastName = person?.LastName,
                Email = person?.Email,
                Phone = person?.Phone,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                IsActive = true
            };
        }

        public List<Com.OldLeaf.Shared.Order.Order> SearchOrders(int customerId, string criteria)
        {
            // Call person proxy to validate customer
            var person = _personProxy.Get(customerId);

            // Call order proxy to search orders
            var orders = _orderProxy.SearchOrder(customerId, OrderType.Online);

            // Call delivery proxy for additional delivery details
            foreach (var order in orders)
            {
                var delivery = _deliveryProxy.GetDeliveryByOrder(order.OrderId, this);
            }

            return orders;
        }

        public Com.OldLeaf.Shared.Person.Person SavePerson(Com.OldLeaf.Shared.Person.Person person)
        {
            // Call person proxy to save person
            _personProxy.Save(person);

            // Call order proxy to update related orders
            var orders = _orderProxy.SearchOrder(person.PersonId, OrderType.Online);

            // Call delivery proxy to update delivery information
            foreach (var order in orders)
            {
                var delivery = _deliveryProxy.GetDeliveryByOrder(order.OrderId, this);
            }

            return person;
        }

        public Com.OldLeaf.Shared.Delivery.Delivery GetDeliveryDate(int orderId, string name)
        {
            // Call order proxy to get order details
            var order = _orderProxy.GetOrder(orderId);

            // Call person proxy to get person details
            var person = _personProxy.Search(name);

            // Call delivery proxy to get delivery information
            var delivery = _deliveryProxy.GetDeliveryByOrder(orderId, this);

            return delivery;
        }
    }
}