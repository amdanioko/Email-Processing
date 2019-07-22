using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderProcessing.Lib.OrderProcessing.Lib;
using static OrderProcessing.Lib.RestockNotices;

namespace OrderProcessing.Lib.UnitTests
{
    [TestClass]
    public class RestockNoticeTest
    {
        [TestMethod]
        public void FromEmail()
        {
            string emailBody = File.ReadAllText("RestockNotices\\Shipment Received - Bigfoot Breweries.txt");
            MailMessage email = new MailMessage
            {
                Subject = "Shipment Received",
                Body = emailBody
            };
            RestockNotices notice = RestockNotices.FromEmail(email);
            RestockDetails details = notice.RestockDetails.First();

            Assert.AreEqual(35, details.ProductID);
            Assert.AreEqual("Steeleye Stout", details.ProductName);
            Assert.AreEqual(30, details.Quantity);
            Assert.AreEqual("Bigfoot Breweries (16)", notice.Supplier);
        }

        [TestMethod]
        public void TestAllEmails()
        {
            foreach (string fpath in Directory.GetFiles("RestockNotices", "*.txt"))
            {
                MailMessage msg = new MailMessage
                {
                    Subject = "Shipment Received",
                    Body = File.ReadAllText(fpath)
                };
                Console.WriteLine(fpath);
                RestockNotices notice = RestockNotices.FromEmail(msg);
                RestockDetails details = notice.RestockDetails.First();
                Assert.IsTrue(details.Quantity > 0);
                Assert.IsTrue(details.ProductID > 0);
                Assert.IsFalse(string.IsNullOrEmpty(details.ProductName));
                Assert.IsNotNull(details.Quantity);
            }
        }
    }
}
