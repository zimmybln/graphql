using System.Linq;
using GraphQL.Types;
using GraphQLTest.Data;

namespace GraphQLTest.Types
{
    public class CustomerGraphType : ObjectGraphType<Customer>
    {
        public CustomerGraphType()
        {
            DefineCommonPropertiesAndFields();

            Field<ListGraphType<InvoiceGraphType>>("invoices", resolve: context =>
            {
                DataSource data = context.UserContext as DataSource;

                return data?.Invoices.Where(i => i.CustomerId == context.Source.Id);
            });
        }

        public CustomerGraphType(DataSource dataSource)
        {
            DefineCommonPropertiesAndFields();

            Field<ListGraphType<InvoiceGraphType>>("invoices", resolve: context =>
            {
                return dataSource?.Invoices.Where(i => i.CustomerId == context.Source.Id);
            });
        }

        private void DefineCommonPropertiesAndFields()
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
