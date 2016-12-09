using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyUnitTestExcerise.Fakes;

namespace MyUnitTestExcerise.Tests
{
    [TestClass()]
    public class CustomerRepositoryTests
    {
        [TestMethod()]
        public void AddTest()
        {
            Customer customer = new Customer();
            customer.Name = "Peter";

            IEmailSender sender = new StubIEmailSender()
            {
                SendEmailString = (content) =>
                {
                    return true;
                }
            };

            CustomerRepository repository = new CustomerRepository(sender);
            Customer result = repository.Add(customer);
            Assert.IsTrue(result != null);
            Assert.AreEqual("Peter", result.Name);
        }

        [TestMethod()]
        public void AddTest2()
        {
            Customer customer = new Customer();
            customer.Name = "Peter";

            IEmailSender sender = new StubIEmailSender()
            {
                SendEmailString = (content) =>
                {
                    return false;
                }
            };

            CustomerRepository repository = new CustomerRepository(sender);
            Customer result = repository.Add(customer);
            Assert.IsTrue(result == null);
        }
    }
}