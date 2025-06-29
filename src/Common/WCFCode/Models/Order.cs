using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class OrderItem
    {
        [DataMember]
        public int OrderItemId { get; set; }
        
        [DataMember]
        public int OrderId { get; set; }
        
        [DataMember]
        public int ProductId { get; set; }
        
        [DataMember]
        public int Quantity { get; set; }
        
        [DataMember]
        public decimal UnitPrice { get; set; }
        
        [DataMember]
        public decimal TotalPrice { get; set; }
    }
}