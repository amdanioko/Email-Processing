using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrderProcessing.Lib.UnitTests
{
    [TestClass]
    public class ShipmentNoticeTest
    {
        [TestMethod]
        public void FromEmail()
        {
            string emailBody = File.ReadAllText("Shipment Notice-Ricardo Adocicados.txt");
            MailMessage email = new MailMessage
            {
                Subject = "Order Shipped",
                Body = emailBody
            };
            ShipmentNotice notice = ShipmentNotice.FromEmail(email);
            Assert.AreEqual(11059, notice.OrderNumber);
            Assert.AreEqual("Ricardo Adocicados", notice.CustomerName);
            Assert.AreEqual("nw.ar@platformbronx.com", notice.EmailTo);
            Assert.AreEqual(107.53M, notice.Tax);
            Assert.AreEqual(86.00M, notice.Shipping);
            Assert.AreEqual(1985.75M, notice.OrderTotal);
            Assert.IsTrue(notice.CustomerComments.StartsWith("Thank you for your business"));
            Assert.AreEqual(3, notice.AddressLines.Count);
            Assert.AreEqual("Av. Copacabana, 267", notice.AddressLines.First()); 
            Assert.AreEqual(3, notice.OrderDetails.Count);

            OrderDetail detail = notice.OrderDetails.Last();
            Assert.AreEqual("Camembert Pierrot", detail.Description);
            Assert.AreEqual(35, detail.Quantity);
            Assert.AreEqual(34M, detail.UnitPrice);
            Assert.AreEqual(1154.30M, detail.Total);
        }

        [TestMethod]
        public void TestAllEmails()
        {
            foreach (string fpath in Directory.GetFiles("ShipmentNotices", "*.txt"))
            {
                MailMessage msg = new MailMessage
                {
                    Subject = "Order Shipped",
                    Body = File.ReadAllText(fpath)
                };
                Console.WriteLine(fpath);
                ShipmentNotice notice = ShipmentNotice.FromEmail(msg);
                Assert.IsTrue(notice.OrderNumber > 0);
                Assert.IsFalse(string.IsNullOrEmpty(notice.CustomerComments));
                Assert.IsFalse(string.IsNullOrEmpty(notice.EmailTo));
                Assert.IsNotNull(notice.AddressLines);
                Assert.IsTrue(notice.AddressLines.Count >= 2);
                Assert.IsTrue(notice.Tax > 0);
                Assert.IsTrue(notice.Shipping >= 0);
                Assert.IsNotNull(notice.OrderDetails);
                Assert.IsTrue(notice.OrderDetails.Count >= 1);
            } 
        }
    }
}
