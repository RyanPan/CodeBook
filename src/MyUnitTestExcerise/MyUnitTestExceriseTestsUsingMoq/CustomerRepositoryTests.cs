using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyUnitTestExcerise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUnitTestExcerise.Tests
{
    [TestClass()]
    public class CustomerRepositoryTests
    {
        public MockRepository MockRepository { get; set; }

        [TestInitialize]
        public void Setup()
        {
            MockRepository = new MockRepository(MockBehavior.Default);
        }

        [TestMethod()]
        public void AddTest()
        {
            Customer customer = new Customer();
            customer.Name = "Peter";

            Mock<IEmailSender> sender = MockRepository.Create<IEmailSender>();
            sender.Setup(m => m.SendEmail(It.IsAny<string>())).Returns(true);

            CustomerRepository repository = new CustomerRepository(sender.Object);
            Customer result = repository.Add(customer);
            Assert.IsTrue(result != null);
            Assert.AreEqual("Peter", result.Name);
        }
    }
}