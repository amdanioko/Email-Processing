using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderProcessing.Lib
{
   public class ProductDetail
    {
        static readonly Regex rxProductDetail = new Regex(@"(.+) \((\d+)\)\t(\d+)\t(.+)");
        internal ProductDetail(string detailLine)
        {

            Match match = rxProductDetail.Match(detailLine);

            ProductName = match.Groups[1].Value;
            ProductID = int.Parse(match.Groups[2].Value);
            Quantity = int.Parse(match.Groups[3].Value);
            Discount = decimal.Parse(match.Groups[4].Value);
        }

        public string ProductName { get; internal set; }
        public int ProductID { get; internal set; }
        public int Quantity { get; internal set; }
        public decimal Discount { get; internal set; }
    }
}
