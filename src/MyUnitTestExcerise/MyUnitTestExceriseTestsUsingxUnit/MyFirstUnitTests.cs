using MyUnitTestExcerise.FakesDemo;
using MyUnitTestExcerise.FakesDemo.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyUnitTestExceriseTestsUsingxUnit
{
    public class MyFirstUnitTests
    {
        [Fact]
        public void SaveShouldUpdateTheRepository()
        {
            // Arrange
            var savedCustomer = default(Customer);
            var repository = new StubICustomerRepository
            {
                SaveOrUpdateCustomer = customer => savedCustomer = customer
            };
            var actualCustomer = new Customer { Id = 1, Name = "Peter", Email = "peter@costoso.com" };
            var viewModel = new CustomerViewModel(actualCustomer, repository);

            // Act
            viewModel.Save();

            // Assert
            Assert.NotNull(savedCustomer);
            Assert.Equal(1, savedCustomer.Id);
            Assert.Equal("Peter", savedCustomer.Name);
        }
    }
}
