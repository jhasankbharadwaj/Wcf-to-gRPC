using System;
using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public int CustomerId { get; set; }
        
        [DataMember]
        public string FirstName { get; set; }
        
        [DataMember]
        public string LastName { get; set; }
        
        [DataMember]
        public string Email { get; set; }
        
        [DataMember]
        public string Phone { get; set; }
        
        [DataMember]
        public string Address { get; set; }
        
        [DataMember]
        public DateTime CreatedDate { get; set; }
    }
}
