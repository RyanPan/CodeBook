using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUnitTestExcerise
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            customer.Name = "Peter";
            IEmailSender sender = new EmailSender();
            CustomerRepository repository = new CustomerRepository(sender);
            Customer result = repository.Add(customer);
            if (result != null)
            {
                Console.WriteLine("Adding customer success! An email is sent to {0}.", result.Name);
            }
            else
            {
                Console.WriteLine("Adding customer failed!");
            }
        }
    }
}
