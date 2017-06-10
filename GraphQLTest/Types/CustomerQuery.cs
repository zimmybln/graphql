using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQLTest.Data;

namespace GraphQLTest.Types
{
    public class CustomerQuery : ObjectGraphType<Customer>
    {
        public CustomerQuery()
        {
            Field<ListGraphType<CustomerGraphType>>("customer",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id", DefaultValue = 0 },
                    new QueryArgument<StringGraphType> { Name = "firstName", DefaultValue = "" },
                    new QueryArgument<StringGraphType> { Name = "lastName", DefaultValue = "" },
                    new QueryArgument<StringGraphType> { Name = "city", DefaultValue = "" }
                ),
                resolve: ResolveCustomers);
        }

        protected virtual IEnumerable<Customer> ResolveCustomers(ResolveFieldContext<Customer> context)
        {
            var data = context.UserContext as DataSource;

            return ResolveCustomers(context, data);
        }

        protected  IEnumerable<Customer> ResolveCustomers(ResolveFieldContext<Customer> resolveFieldContext, DataSource data)
        {
            var idValue = resolveFieldContext.GetArgument<int>("id");
            var firstNameValue = resolveFieldContext.GetArgument<string>("firstName");
            var lastNameValue = resolveFieldContext.GetArgument<string>("lastName");
            var cityValue = resolveFieldContext.GetArgument<string>("city");

            StringBuilder query = new StringBuilder();

            if (idValue > 0)
            {
                query.AppendFormat($"Id == {idValue}");
            }

            if (!String.IsNullOrWhiteSpace(firstNameValue))
            {
                if (query.Length > 0)
                    query.Append(" AND ");
                query.AppendFormat("FirstName.Contains({0}{1}{0})", (char)34, firstNameValue);
            }

            if (!String.IsNullOrWhiteSpace(lastNameValue))
            {
                if (query.Length > 0)
                    query.Append(" AND ");

                query.AppendFormat("LastName.Contains({0}{1}{0})", (char)34, lastNameValue);
            }

            if (!String.IsNullOrWhiteSpace(cityValue))
            {
                if (query.Length > 0)
                    query.Append(" AND ");

                query.AppendFormat("City.Contains({0}{1}{0})", (char)34, cityValue);
            }

            if (query.Length == 0)
                return data.Customers;

            return data.Customers.Where(query.ToString()).ToArray(); ;
        }

    }
}
