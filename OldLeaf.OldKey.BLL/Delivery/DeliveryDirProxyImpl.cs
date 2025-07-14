using System;
using Com.OldLeaf.Shared.Delivery;
using Com.OldLeaf.Shared.Customer;
using OldLeaf.Services;

namespace OldLeaf.OldKey.BLL.Delivery
{
    public class DeliveryDirProxyImpl : IDeliveryDirProxy
    {
        private readonly SqlDataServices _dataServices;

        public DeliveryDirProxyImpl()
        {
            _dataServices = new SqlDataServices();
        }

        public DeliveryDirProxyImpl(string connectionString)
        {
            _dataServices = new SqlDataServices(connectionString);
        }

        public Com.OldLeaf.Shared.Delivery.Delivery GetDeliveryByOrder(int orderId, ICustomerDirProxy customerProxy)
        {
            // Get delivery information from data services
            var delivery = _dataServices.Get<Com.OldLeaf.Shared.Delivery.Delivery>(orderId, "OrderId = @orderId");

            // Use customer proxy to get additional customer information if needed
            if (delivery != null)
            {
                // You can call customerProxy methods here if needed for additional processing
                // var customer = customerProxy.GetCustomer(delivery.CustomerId);
            }

            return delivery ?? new Com.OldLeaf.Shared.Delivery.Delivery
            {
                OrderId = orderId,
                DeliveryDate = DateTime.Now.AddDays(7),
                ExpectedDeliveryDate = DateTime.Now.AddDays(5),
                DeliveryStatus = "Pending",
                TrackingNumber = $"TRK{orderId}{DateTime.Now:yyyyMMdd}",
                DeliveryMethod = "Standard",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                IsActive = true
            };
        }
    }
}
