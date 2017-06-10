using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQLTest.Data;

namespace GraphQLTest.Types
{
    /// <summary>
    /// This Graphtype allows to add a new customer
    /// </summary>
    public class CustomerMutation : ObjectGraphType<Customer>
    {
        public CustomerMutation()
        {
            Field<CustomerGraphType>("createCustomer",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "firstName", DefaultValue = ""},
                    new QueryArgument<StringGraphType> {Name = "lastName", DefaultValue = ""},
                    new QueryArgument<StringGraphType> {Name = "city", DefaultValue = ""}
                ), resolve: OnCreateCustomer);
        }

        private Customer OnCreateCustomer(ResolveFieldContext<Customer> context)
        {
            var data = context.UserContext as DataSource;

            if (data == null)
                return null;

            var firstName = context.GetArgument<string>("firstName");
            var lastName = context.GetArgument<string>("lastName");
            var city = context.GetArgument<string>("city"); 

            var customer = new Customer()
            {
                FirstName = firstName,
                LastName = lastName,
                City = city,
                Id = data.Customers.Max(c => c.Id) + 1
            };

            data.Customers.Add(customer);

            return customer;
        }
    }
}
