using System.Runtime.Serialization;

namespace xyz.Data
{
    [DataContract]
    public class OrderItem
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
