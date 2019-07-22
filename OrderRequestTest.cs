using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static OrderProcessing.Lib.OrderRequest;

namespace OrderProcessing.Lib.UnitTests
{
    [TestClass]
    public class OrderRequestTest
    {
        [TestMethod]
        public void FromEmail()
        {
            string emailBody = File.ReadAllText("OrderRequests\\Order Request-Antonio Moreno Taquería.txt");
            MailMessage email = new MailMessage
            {
                Subject = "Order Request",
                Body = emailBody
            };
            OrderRequest orderReq = OrderRequest.FromEmail(email);
            DateTime date    = new DateTime(2018, 12, 04);
            DateTime dateII  = new DateTime(2018, 12, 11);
            DateTime dateIII =  DateTime.Now.Date.AddDays(7);

            Assert.AreEqual("Antonio Moreno Taquería", orderReq.CustomerName);
            Assert.AreEqual("Michael Suyama", orderReq.SalesRepName);
            Assert.AreEqual(6, orderReq.SalesRepID);
            Assert.AreEqual(date, orderReq.OrderDate);
            Assert.AreEqual(dateII, orderReq.RequiredDate);
            Assert.AreEqual(1, orderReq.ShipperID);
            Assert.AreEqual(1554.98m,orderReq.Freight);
            Assert.AreEqual("Speedy Express", orderReq.ShipperName);
            Assert.AreEqual(8, orderReq.ProductDetails.Count);

            ProductDetail product = orderReq.ProductDetails.Last();

            Assert.AreEqual("Röd Kaviar", product.ProductName);
            Assert.AreEqual(73M, product.ProductID);
            Assert.AreEqual(30M, product.Quantity);
            Assert.AreEqual(0.03M, product.Discount);

        }
        [TestMethod]
        public void TestAllEmails()
        {
            foreach(string fpath in Directory.GetFiles("OrderRequests", "*.txt"))
            {
                Console.WriteLine(fpath);
                MailMessage msg = new MailMessage
                {
                    Subject = "Order Request",
                    Body = File.ReadAllText(fpath)
                };
                OrderRequest orderReq = OrderRequest.FromEmail(msg);

                Assert.IsFalse(string.IsNullOrEmpty(orderReq.CustomerName));
                Assert.IsFalse(string.IsNullOrEmpty(orderReq.SalesRepName));
                Assert.IsNotNull(orderReq.OrderDate);
                Assert.IsNotNull(orderReq.RequiredDate);
                Assert.IsFalse(string.IsNullOrEmpty(orderReq.ShipperName)); ;
                Assert.IsTrue(orderReq.Freight > 0);


                Assert.IsTrue(orderReq.ProductDetails.All(p => !string.IsNullOrEmpty(p.ProductName)));
                Assert.IsTrue(orderReq.ProductDetails.All(p => p.Discount >= 0));
                Assert.IsTrue(orderReq.ProductDetails.All(p => p.Quantity > 0));

            }
        }
    }
}
