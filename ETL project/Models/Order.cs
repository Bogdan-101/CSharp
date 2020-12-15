using System;
using System.Data.SqlTypes;

namespace Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string customer { get; set; }
        public string shipName { get; set; }
        public string shipAddress { get; set; }
        public string shipCity { get; set; }
        public string shipCountry { get; set; }
        public DateTime? shippedDate { get; set; }
    }
}
