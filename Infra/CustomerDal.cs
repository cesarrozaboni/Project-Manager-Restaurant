using Domain.Models;
using Infra.DatabaseModel;
using System.Collections.Generic;
using System.Linq;

namespace Infra
{
    public class CustomerDal
    {
        private RestauranteDBEntities objRestauranteDBEntities;

        public RestauranteDBEntities RestauranteDBEntities
        {
            get { return objRestauranteDBEntities ?? (objRestauranteDBEntities = new RestauranteDBEntities()); }
        }

        public CustomerDal()
        {
        }

        public List<CustomersDomain> GetAllCustomers()
        {
            List<CustomersDomain> customers = (from obj in RestauranteDBEntities.Customers
                                               select new CustomersDomain()
                                               {
                                                    CustomerName = obj.CustomerName,
                                                    CustomerId   = obj.CustomerId
                                               }).ToList();

            return customers;
        }

    }
}
