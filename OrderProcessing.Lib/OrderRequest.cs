using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderProcessing.Lib
{
    public class OrderRequest
    {
        private static readonly string[] delimiters = new string[] { "\r\n" };
        private static readonly Regex rxOrderReqFields = new Regex(@":\t(.*)\s", RegexOptions.Compiled);
        private static readonly Regex rxCustomer = new Regex(@":\t(.+)\s\((\w+)\)", RegexOptions.Compiled);
        private static readonly Regex rxEmployee = new Regex(@":\t(.+)\s\((\d+)\)", RegexOptions.Compiled);
        private static readonly Regex rxProduct = new Regex(@"(.*)\t(.*)\t(.*)", RegexOptions.Compiled);
        private static readonly Regex rxDate = new Regex(@":\t(\d.+)", RegexOptions.Compiled);

        public static OrderRequest FromEmail(MailMessage email)
        {
            if (email is null) throw new ArgumentNullException(nameof(email));
            if (email.Subject != "Order Request") throw new ArgumentException($"Wrong Email Type: {email.Subject}");

            MatchCollection matches = rxOrderReqFields.Matches(email.Body);
            if (matches.Count != 7) throw new ArgumentException("Incorrect # matches for order fields.");
            Match match = rxCustomer.Match(matches[0].Value);

            OrderRequest orderReq = new OrderRequest
            {
                CustomerName = match.Groups[1].Value,
                CustomerId = match.Groups[2].Value

            };
            if  (matches[6].Groups[1].Value.Trim() == "default")
            {
                orderReq.OrderDate = DateTime.Now.AddDays(7);
            }

            match = rxEmployee.Match(matches[1].Value);

            orderReq.SalesRepName = match.Groups[1].Value;
            orderReq.SalesRepID = int.Parse(match.Groups[2].Value);
            orderReq.ShipperName = match.Groups[1].Value;

            match = rxDate.Match(matches[2].Value);
            orderReq.OrderDate = DateTime.Parse(match.Groups[1].Value);
            match = rxDate.Match(matches[3].Value);
            orderReq.RequiredDate = DateTime.Parse(match.Groups[1].Value);

            match = rxOrderReqFields.Match(matches[5].Value);
            orderReq.Freight = decimal.Parse(match.Groups[1].Value.Substring(1));

            match = rxEmployee.Match(matches[4].Value);
            orderReq.ShipperName = match.Groups[1].Value;
            orderReq.ShipperID = int.Parse(match.Groups[2].Value);

            string[] lines = email.Body.Split(delimiters, StringSplitOptions.None);
            for (int nLine = 0; nLine < lines.Length; nLine++)
            {
                if (lines[nLine].StartsWith("Product"))
                {
                    List<ProductDetail> details = lines.Skip(nLine + 1).TakeWhile(l => l != "").Select(l => new ProductDetail(l)).ToList();
                    orderReq.ProductDetails = details.AsReadOnly();
                }
                
                continue;
            }
            return orderReq;
        }
        public string CustomerName { get; private set; }
        public string CustomerId { get; private set; }
        public string SalesRepName { get; private set; }
        public string ShipperName { get; private set; }
        public DateTime OrderDate { get; private set; }
        public DateTime RequiredDate { get; private set; }
        public decimal Freight { get; private set; }
        public int ShipperID { get; private set; }
        public int SalesRepID { get; private set; }
        public IReadOnlyCollection < ProductDetail> ProductDetails { get; private set; }
    
    }
}
