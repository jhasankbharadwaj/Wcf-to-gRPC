namespace Models
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public int ProductId { get; set; }
        
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public string Description { get; set; }
        
        [DataMember]
        public decimal Price { get; set; }
        
        [DataMember]
        public int StockQuantity { get; set; }
        
        [DataMember]
        public string Category { get; set; }
        
        [DataMember]
        public DateTime CreatedDate { get; set; }
    }
}