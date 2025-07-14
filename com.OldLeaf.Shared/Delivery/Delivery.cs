using System;

namespace Com.OldLeaf.Shared.Delivery
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryStatus { get; set; }
        public string TrackingNumber { get; set; }
        public string DeliveryMethod { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}