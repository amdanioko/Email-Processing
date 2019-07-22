using System;
using System.IO;
using System.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrderProcessing.Lib.UnitTests
{
    [TestClass]
    public class PaymentReceivedTest
    {
        [TestMethod]
        public void FromEmail()
        {
           foreach (string fpath in Directory.GetFiles("PaymentReceived", "*.txt"))
            {
                MailMessage email = new MailMessage
                {
                    Subject = "Payment Received",
                    Body = File.ReadAllText(fpath)
                    
                };
                PaymentReceived notice = PaymentReceived.FromEmail(email);

            }
           
        }
    }
}