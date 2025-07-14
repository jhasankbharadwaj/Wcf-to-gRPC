using System;

namespace Com.OldLeaf.Shared.Customer
{
    public class CustomerDemographics
    {
        public int CustomerId { get; set; }
        public string AgeGroup { get; set; }
        public string IncomeLevel { get; set; }
        public string MaritalStatus { get; set; }
        public string Education { get; set; }
        public string Occupation { get; set; }
        public string PreferredLanguage { get; set; }
        public string Region { get; set; }
        public string CustomerSegment { get; set; }
        public decimal PurchasingPower { get; set; }
        public string LifestyleCategory { get; set; }
    }
}