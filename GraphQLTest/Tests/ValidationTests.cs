using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQLTest.Data;
using GraphQLTest.Types;
using NUnit.Framework;

namespace GraphQLTest.Tests
{
    [TestFixture]
    public class ValidationTests : TestBase
    {
        [Test]
        public void QueryWithFragments()
        {
            var schema = new Schema { Query = new CustomerQuery() };

            string query = @"
                            query {
                                unknowntype {
                                    id
                                    ...fullname
                                } 
                            }

                            fragment fullname on customer {
                                firstName
                                lastName
                            }";

            Task<ExecutionResult> task = Validate(query, schema);
            task.Wait();

            var result = task.Result;

            if (result.Errors != null && result.Errors.Any())
            {
                int counter = 1;

                foreach (var executionError in result.Errors)
                {
                    Console.WriteLine($"{counter:00} {executionError.Message}");
                    counter++;
                }
            }
            else
            {
                Console.WriteLine("not any errors");
            }

        }
    }
}
