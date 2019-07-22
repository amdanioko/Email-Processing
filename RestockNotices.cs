using OrderProcessing.Lib.OrderProcessing.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderProcessing.Lib
{
    public class RestockNotices //This class is for parsing all of the email information in the restock notice text file
    {    
            private static readonly char[] delimiters = new char[] { '\t' };
            private static readonly Regex rxproductItems = new Regex(@"(.*)\t(.*)\t(\d+)", RegexOptions.Compiled);
            private static readonly Regex rxFullDetails = new Regex(@"supplier (.+)\:", RegexOptions.Compiled);
    
  
            public static RestockNotices FromEmail(MailMessage email)
            {
                if (email is null) throw new ArgumentNullException(nameof(email));
                if (email.Subject != "Shipment Received")
                    throw new ArgumentNullException($"Wrong email type: {email.Subject}");

                MatchCollection myCollection = rxproductItems.Matches(email.Body);
                Match myCollection2 = rxFullDetails.Match(email.Body);

            RestockNotices notice = new RestockNotices
            {
                    Supplier = myCollection2.Groups[1].Value
                };

                MatchCollection myMatchCollection = rxproductItems.Matches(email.Body);
                List<RestockDetails> RestockDetailsList = new List<RestockDetails>();

                foreach (Match a in myMatchCollection)
                {
                    RestockDetails myDetails = new RestockDetails(a.Value);
                    RestockDetailsList.Add(myDetails);
                }
                notice.RestockDetails = RestockDetailsList;
                return notice;
            }

            public int ProductID { get; private set; }
            public string ProductName { get; private set; }
            public int Quantity { get; private set; }
            public string Supplier { get; private set; }
            public IReadOnlyCollection<RestockDetails> RestockDetails { get; private set; }
        
    }
    namespace OrderProcessing.Lib
    {
        public class RestockDetails
        {
            internal RestockDetails(string detailLine)
            {
                string[] parts = detailLine.Split('\t');
                ProductID = int.Parse(parts[0]);
                ProductName = parts[1];
                Quantity = int.Parse(parts[2]);
            }

            public int ProductID { get; private set; }
            public string ProductName { get; private set; }
            public int Quantity { get; private set; }

        }
    }
}