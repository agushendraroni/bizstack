using System.Net.Http.Json;
using FrameworkX.Services.GraphQLGateway.Types;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;





namespace FrameworkX.Services.GraphQLGateway.Queries
{
    public class UserQuery
    {
         public List<string> GetUsers() => new() { "Alice", "Bob", "Charlie" };
        public async Task<List<User>> GetUsersAsync([Service] IHttpClientFactory clientFactory)
        {
            var client = clientFactory.CreateClient("user-service");
            return await client.GetFromJsonAsync<List<User>>("/api/users");
        }
    }
}
