using FrameworkX.Services.GraphQLGateway.Queries;
using FrameworkX.Services.GraphQLGateway.Queryplan;
using FrameworkX.Services.GraphQLGateway.Types;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// builder.Services
//     .AddGraphQLServer()
//     .AddQueryType<UserQuery>()
//     .AddDiagnosticEventListener<QueryPlanLoggingListener>(); // Hapus .UsePersistedQueryPipeline()


builder.Services
    .AddGraphQLServer()
    .AddQueryType<UserQuery>()
    .AddType<User>()  // penting register entity type
    .AddApolloFederation();


builder.Services.AddHttpClient("user-service", client =>
{
    client.BaseAddress = new Uri("http://localhost:5282");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL(); // GraphQL endpoint di /graphql
});

app.MapBananaCakePop("/graphql/ui"); // UI GraphQL di /graphql/ui

Console.WriteLine("GraphQL Gateway running at http://localhost:5000/graphql");
Console.WriteLine("Banana Cake Pop UI running at http://localhost:5000/graphql/ui");

app.Run();
