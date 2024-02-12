using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tudu.Api.Features.Todos.ViewModels;
using Tudu.Api.Shared.Endpoint;
using Tudu.Infrastructure.Data;
using Tudu.Infrastructure.Extensions;

namespace Tudu.Api.Features.Todos
{
    public class GetTodosEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("/todo", (IMediator mediator, [FromQuery] DateOnly? assignedDate) =>
                {
                    return mediator.Send(new GetTodosQuery(assignedDate));
                })
                .WithName(nameof(GetTodosEndpoint));
            return routeBuilder;
        }
    }

    public record GetTodosQuery(DateOnly? AssignedDate) : IRequest<GetTodosResponse>;

    // TODO: to paged result
    public class GetTodosResponse
    {
        public TodoViewModel[] Todos { get; set; }
    }

    public class GetTodosHandler : IRequestHandler<GetTodosQuery, GetTodosResponse>
    {
        private readonly TuduDbContext _dbContext;

        public GetTodosHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetTodosResponse> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            var todoViewModels = await _dbContext
                .Todos
                .WhereIf(request.AssignedDate.HasValue, todo => todo.AssignedDate == request.AssignedDate)
                .Select(todo => TodoViewModel.From(todo))
                .ToArrayAsync();

            return new GetTodosResponse()
            {
                Todos = todoViewModels
            };
        }
    }
}
