using GraphQL.Types;
using GraphQLTest.Data;

namespace GraphQLTest.Types
{
    public class InvoiceGraphType : ObjectGraphType<Invoice>
    {
        public InvoiceGraphType()
        {
            Field(x => x.Id);
            Field(x => x.CustomerId);
            Field(x => x.Date);
            Field(x => x.Price);
        }

        public InvoiceGraphType(DataSource data) : this()
        {
            
        }
    }
}
