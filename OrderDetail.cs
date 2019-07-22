using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Lib
{
    public class OrderDetail // A nested class
    {

        internal OrderDetail(string detailLine)
        {
            string[] parts = detailLine.Split('\t');
            Description = parts[0];
            Quantity = int.Parse(parts[1]);
            UnitPrice = decimal.Parse(parts[2].Substring(1));
            Total = Decimal.Parse(parts[3].Substring(1));
        }
        public string Description { get; internal set; } 
        // any class in the assembly can access this,
        //Thats the purpose of using internal instead of private inside the curly braces
        public int Quantity { get; internal set; }
        public decimal UnitPrice { get; internal set; }
        public decimal Total { get; internal set; }
    }
}
