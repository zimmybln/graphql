using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQLTest.Data;

namespace GraphQLTest.Types.Simple
{
    public class CustomerQuery : ObjectGraphType<Customer>
    {
        public CustomerQuery()  
        {
            Field<ListGraphType<CustomerGraphType>>("customer",
                resolve: ResolveCustomers);
        }

        protected  IEnumerable<Customer> ResolveCustomers(ResolveFieldContext<Customer> context)
        {
            var data = context.UserContext as DataSource;

            return data?.Customers;
        }
    }
}
