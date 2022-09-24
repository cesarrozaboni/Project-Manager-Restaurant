using Domain.Models;
using Infra;
using System.Collections.Generic;

namespace Services.Repositories
{
    public class CustomerRepository
    {
        public CustomerRepository()
        {
        }

        public List<CustomersDomain> GetAllCustomers()
        {
            CustomerDal customer = new CustomerDal();
            return customer.GetAllCustomers();
        }
    }
}