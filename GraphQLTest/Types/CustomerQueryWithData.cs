using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQLTest.Data;

namespace GraphQLTest.Types
{
    public class CustomerQueryWithData : CustomerQuery
    {
        private readonly DataSource _data;

        public CustomerQueryWithData(DataSource data)
        {
            _data = data;
        }

        protected override IEnumerable<Customer> ResolveCustomers(ResolveFieldContext<Customer> resolveFieldContext)
        {
            return ResolveCustomers(resolveFieldContext, _data);
        }
    }
}
