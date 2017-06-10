using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQLTest.Data;

namespace GraphQLTest.Types.Simple
{
    public class CustomerGraphType : ObjectGraphType<Customer>
    {
        public CustomerGraphType()
        {
            Name = "customer";
            Description = "a person or company";

            Field(x => x.Id);
            Field(x => x.FirstName);
            Field(x => x.LastName);
            Field(x => x.City);
        }

    }
}
