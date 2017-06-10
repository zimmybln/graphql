using GraphQLTest.Data;
using GraphQLTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Type = System.Type;

namespace GraphQLTest.Tests
{
    [TestFixture]
    public class QueryTests : TestBase
    {

        [Test]
        public async Task QueryAll()
        {
            var schema = new Schema { Query = new CustomerQuery() };

            string query = @"
                            query {
                                customer {
                                    id
                                    firstName
                                    lastName
                                }                                
                            }";

            var result = await Query(query, schema, new DataSource());

            Write(result);
            Assert.IsNull(result.Errors, "There were any errors");

        }

        [Test]
        public async Task QueryWithFragments()
        {
            var schema = new Schema { Query = new CustomerQuery() };

            string query = @"
                            query {
                                customer {
                                    id
                                    ...fullname
                                } 
                            }

                            fragment fullname on customer {
                                firstName
                                lastName
                            }
                            ";

            var result = await Query(query, schema, new DataSource());

            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);
        }

        #region Introspection

        // http://graphql.org/learn/introspection/

        [Test]
        public async Task QueryForMetadataTypename()
        {
            var schema = new Schema { Query = new CustomerQuery() };

            string query = @"
                            query {
                                customer {
                                    __typename
                                }                                
                            }";

            var result = await Query(query, schema, new DataSource());

            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);
        }

        [Test]
        public async Task QueryForMetadataSchemaTypes()
        {
            var schema = new Schema { Query = new CustomerQuery() };

            string query = @"
                            query askforschema {
                                __schema {
                                    types {
                                        name
                                        fields {
                                            name
                                        }
                                    }
                                }
                            }";

            var result = await Query(query, schema, new DataSource());

            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);
        }

        [Test]
        public async Task QueryForMetadataSchemaQueryType()
        {
            var schema = new Schema { Query = new CustomerQuery() };

            string query = @"
                            query askforschema {
                                __schema {
                                    queryType {
                                        name
                                    }
                                }
                            }";

            var result = await Query(query, schema, new DataSource());

            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);
        }

        [Test]
        public async Task QueryForMetadataType()
        {
            var schema = new Schema { Query = new CustomerQuery() };

            string query = @"
                            query askfortype {
                                __type (name: ""customer"") {
                                    name
                                    description
                                    kind
                                    fields {
                                        name
                                        type {
                                            name
                                            kind
                                            ofType {
                                                name
                                                kind
                                            }
                                        }
                                        args {
                                            name
                                        }
                                    }
                                }
                            }";

            var result = await Query(query, schema, new DataSource());

            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);
        }

        #endregion

        [Test]
        public async Task QueryWithParameters()
        {
            var schema = new Schema { Query = new CustomerQuery() };

            string queryWithParameter = @"
                            query {
                                customer (firstName: ""Sim"") {
                                    id
                                    firstName
                                    lastName
                                }
                            }";

            var result = await Query(queryWithParameter, schema, new DataSource());

            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);
        }

        [Test]
        public async Task QueryWithAliases()
        {
            var schema = new Schema { Query = new CustomerQuery() };
            var inputs = new Inputs();
            
            string queryWithParameter = @"
                            query {
                                norddeutsche : customer (city : ""Hamburg"") {
                                    id
                                    firstName
                                    lastName
                                    city
                                }

                                süddeutsche : customer (city : ""München"") {
                                    id
                                    firstName
                                    lastName
                                    city
                                }
                            }";

            var result = await Query(queryWithParameter, schema, new DataSource(), inputs);

            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);
        }

        [Test]
        public async Task QueryWithVariables()
        {
            var schema = new Schema {Query = new CustomerQuery()};
            var inputs = new Inputs();
            inputs.Add("firstName", "S");

            string queryWithParameter = @"
                            query CustomerQuery ($firstName : String) {
                                customer (firstName: $firstName) {
                                    id
                                    firstName
                                    lastName
                                }
                            }";

            var result = await Query(queryWithParameter, schema, new DataSource(), inputs);
            
            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);
        }

        [Test]
        public async Task QueryWithRelation()
        {
            var schema = new Schema { Query = new CustomerQuery() };

            string queryWithRelation = @"
                        query {
                            customer (id: 1) {
                                id
                                firstName
                                lastName
                                invoices
                                {
                                    id
                                    customerId
                                    date
                                    price
                                }
                            }

                        }";

            var result = await Query(queryWithRelation, schema, new DataSource());

            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);
        }

        [Test]
        public async Task QueryWithDirectives()
        {
            var schema = new Schema { Query = new CustomerQuery() };
            var inputs = new Inputs();
            inputs.Add("withInvoices", "true");

            string queryWithRelation = @"
                        query {
                            customer {
                                id
                                firstName
                                lastName
                                invoices @include(if: true)
                                {
                                    id
                                    customerId
                                    date
                                    price
                                }
                            }
                        }";

            // the result is not as expected -> bug or misunderstood feature?

            var result = await Query(queryWithRelation, schema, new DataSource(), inputs);

            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);

        }

        [Test]
        public async Task QueryWithCustomFactory()
        {
            var datasource = new DataSource();

            // define the unity container
            var container = new UnityContainer();

            container.RegisterInstance(typeof(DataSource), datasource);
            container.RegisterType<CustomerGraphType>();
            container.RegisterType<InvoiceGraphType>();
            container.RegisterType<CustomerQueryWithData>();
            
            // callback for resolving types with parameters
            IGraphType OnResolveType(Type type)
            {
                return container.Resolve(type) as IGraphType;
            }
            
            // schema with resolving function
            var schema = new Schema
            {
                Query = container.Resolve(typeof(CustomerQueryWithData)) as IObjectGraphType,
                ResolveType = OnResolveType
            };

            // query definition
            string queryWithRelation = @"
                        query {
                            customer {
                                id
                                firstName
                                lastName
                                invoices
                                {
                                    id
                                    customerId
                                    date
                                    price
                                }
                            }
                        }";

            var result = await Query(queryWithRelation, schema);

            Assert.IsNull(result.Errors, "There were any errors");
            Write(result);
        }


    }
}
