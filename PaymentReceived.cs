using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderProcessing.Lib
{
    public class PaymentReceived
    {
        private static readonly Regex rxAmount = new Regex(@"\$(\d+\,?\d+\.?\d+)", RegexOptions.Compiled);
        private static readonly Regex rxOrderNumber = new Regex(@"order\s(\d+)", RegexOptions.Compiled);
        private static readonly Regex rxDate = new Regex(@"on\s(\d+\-\d+\-\d+)", RegexOptions.Compiled);
        private static readonly Regex rxCheckNum = new Regex(@"is\s(\d+)", RegexOptions.Compiled);

        public static PaymentReceived FromEmail(MailMessage email)
        {
            if (email is null) throw new ArgumentNullException(nameof(email));

            if (email.Subject != "Payment Received") throw new ArgumentException($"Wrong email type:{email.Subject}");

            Match mAmount = rxAmount.Match(email.Body);
            Match mOrderN = rxOrderNumber.Match(email.Body);
            Match mDate = rxDate.Match(email.Body);
            Match mCheckNum = rxCheckNum.Match(email.Body);
            if (!mCheckNum.Success) throw new ArgumentException("Invalid email body");

            PaymentReceived notice = new PaymentReceived
            {
                OrderNumber = int.Parse(mOrderN.Groups[1].Value),

                Amount = decimal.Parse(mAmount.Groups[1].Value),

                Date = DateTime.Parse(mDate.Groups[1].Value),

                CheckN = int.Parse(mCheckNum.Groups[1].Value),

            };
            return notice;
        }


        public int CheckN { get; private set; }
        public DateTime Date { get; private set; }
        public int OrderNumber { get; private set; }
        public decimal Amount { get; private set; }
    }
}
