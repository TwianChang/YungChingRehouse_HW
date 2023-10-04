using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YungChingRehouse_HW.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string ContactName { get; set; }
        public string CustomerID { get; set; }
        public string ProductName { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmployeeID { get; set; }
        public DateTime OrderDate { get; set; }
    }
}