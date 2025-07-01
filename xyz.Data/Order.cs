using System.Runtime.Serialization;

namespace xyz.Data
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public List<OrderItem> Items { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public decimal TotalAmount { get; set; }
    }
}
