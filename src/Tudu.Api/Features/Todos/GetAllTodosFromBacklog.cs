using Tudu.Api.Shared.Endpoint;

namespace Tudu.Api.Features.Todos
{
    public class GetAllTodosFromBacklog
    {
        public class Endpoint : IEndpoint
        {
            public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
            {
                routeBuilder
                    .MapGet("/todo/backlog", () =>
                    {
                        return Results.Ok();
                    })
                    .WithName(nameof(GetAllTodosFromBacklog));
                return routeBuilder;
            }
        }

        public class Query
        {

        }

        public class Validator
        {

        }

        public class Handler
        {

        }
    }
}
