using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLTest.Types
{

    public class Customer : Simple.Customer
    {
        public IEnumerable<Invoice> Invoices { get; set; }
    }
}
