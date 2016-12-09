using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUnitTestExcerise
{
    public class CustomerRepository : ICustomerRepository
    {
        private IEmailSender emailSender;

        public CustomerRepository(IEmailSender sender)
        {
            emailSender = sender;
        }

        public Customer Add(Customer customer)
        {
            bool sent = emailSender.SendEmail(customer.ToString());
            if (sent)
            {
                return customer;
            }
            else
            {
                return null;
            }
        }
    }
}
