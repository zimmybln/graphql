using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Rules;
using GraphQLTest.Data;
using GraphQLTest.Types;

namespace GraphQLTest.Tests
{
    /// <summary>
    /// This class some methods to execute various types of queries
    /// </summary>
    public abstract class TestBase
    { 
        protected static async Task<ExecutionResult> Query(string query, Schema schema, object userContext = null, Inputs inputs = null, System.Type middlewaretype = null)
        {
            var result = await new DocumentExecuter().ExecuteAsync(options =>
            {
                options.Schema = schema;
                options.UserContext = userContext;
                options.Inputs = inputs; 
                options.Query = query;
                if (middlewaretype != null)
                {
                    options.FieldMiddleware.Use(middlewaretype);
                }
            }).ConfigureAwait(false);

            return result;
        }

        protected static async Task<ExecutionResult> Validate(string query, Schema schema, Inputs inputs = null)
        {
            return await new DocumentExecuter().ExecuteAsync(options =>
            {
                options.Query = query;
                options.Schema = schema;
                options.UserContext = new DataSource();
                options.Inputs = inputs;
                options.ValidationRules = DocumentValidator.CoreRules();
            });
        }

        protected void Write(ExecutionResult result)
        {
            var resultasstring = new DocumentWriter(indent:true).Write(result);
            Console.WriteLine(resultasstring);
        }
    }
}
