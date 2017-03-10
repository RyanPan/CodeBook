using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUnitTestExcerise.FakesDemo
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public interface ICustomerRepository
    {
        Customer GetById(int Id);
        IEnumerable<Customer> GetAll();
        Customer SaveOrUpdate(Customer customer);
        void Delete(Customer customer);
    }

    public class CustomerRepository : ICustomerRepository
    {
        public void Delete(Customer customer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAll()
        {
            throw new NotImplementedException();
        }

        public Customer GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Customer SaveOrUpdate(Customer customer)
        {
            throw new NotImplementedException();
        }
    }

    public class ViewModelBase
    {

    }

    public class CustomerViewModel : ViewModelBase
    {
        private Customer customer;
        private readonly ICustomerRepository repository;

        public CustomerViewModel(Customer customer, ICustomerRepository repository)
        {
            this.customer = customer;
            this.repository = repository;
        }

        public string Name
        {
            get { return this.customer.Name; }
            set
            {
                customer.Name = value;
                // RaisePropertyChanged("Name");
            }
        }

        public void Save()
        {
            customer = repository.SaveOrUpdate(customer);
        }
    }
}
