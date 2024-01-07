using Tudu.Api.Shared.Endpoint;

namespace Tudu.Api.Features.Todos
{
    public class AddTodoToBacklog
    {
        public class Endpoint : IEndpoint
        {
            public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
            {
                routeBuilder
                    .MapPost("/todo/backlog", () =>
                    {
                        return Results.Ok();
                    })
                    .WithName(nameof(AddTodoToBacklog));
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
