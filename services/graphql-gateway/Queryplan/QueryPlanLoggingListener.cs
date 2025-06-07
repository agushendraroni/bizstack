using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Language;
using Microsoft.Extensions.Logging;
using System;

namespace FrameworkX.Services.GraphQLGateway.Queryplan
{
    public class QueryPlanLoggingListener : ExecutionDiagnosticEventListener
    {
        private readonly ILogger<QueryPlanLoggingListener> _logger;

        public QueryPlanLoggingListener(ILogger<QueryPlanLoggingListener> logger)
        {
            _logger = logger;
        }

        public override IDisposable ExecuteRequest(IRequestContext context)
        {
            _logger.LogInformation("🚀 Executing GraphQL Request");

            // Log query as printed document
            if (context?.Document != null)
            {
                string printedQuery = context.Document.ToString();
                _logger.LogInformation("🔍 Query Document:\n{Query}", printedQuery);
            }

            if (context?.Operation != null)
            {
                _logger.LogInformation("📌 Operation Name: {OperationName}", context.Operation.Name ?? "(anonymous)");
            }

            return ExecutionDiagnosticEventListener.EmptyScope;
        }
    }
}
