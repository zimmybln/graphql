using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQLTest.Types;

namespace GraphQLTest.Data
{
    public class DataSource
    {
        public List<Customer> Customers { get; } = new List<Customer>()
        {
            new Customer(){Id = 1, FirstName = "Hans", LastName = "Maier", City = "Berlin"},
            new Customer(){Id = 2, FirstName = "Simone", LastName = "Selmke", City = "Berlin"},
            new Customer(){Id = 3, FirstName = "Simone", LastName = "Schulze", City = "Dresden"},
            new Customer(){Id = 4, FirstName = "Peter", LastName = "Traurig", City = "München"},
            new Customer(){Id = 5, FirstName = "Stephan", LastName = "Savatzki", City = "München"},
            new Customer(){Id = 6, FirstName = "Alexandra", LastName = "Peters", City = "Hamburg"},
        };

        public List<Invoice> Invoices { get; } = new List<Invoice>()
        {
            new Invoice() {Id = 1, CustomerId = 1, Date = new DateTime(2017, 1, 10), Price = 50},
            new Invoice() {Id = 2, CustomerId = 1, Date = new DateTime(2017, 5, 2), Price = 150},
            new Invoice() {Id = 3, CustomerId = 2, Date = new DateTime(2017, 3, 10), Price = 80},
            new Invoice() {Id = 4, CustomerId = 2, Date = new DateTime(2017, 10, 10), Price = 19.99},
        };
    }
}
