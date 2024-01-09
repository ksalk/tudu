using MediatR;
using Tudu.Api.Shared.Endpoint;
using Tudu.Domain.Enums;
using Tudu.Infrastructure.Data;

namespace Tudu.Api.Features.Todos
{
    public class GetAllTodosFromBacklogEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("/todo/backlog", (IMediator mediator) =>
                {
                    return mediator.Send(new GetAllTodosFromBacklogQuery());
                })
                .WithName(nameof(GetAllTodosFromBacklogEndpoint));
            return routeBuilder;
        }
    }

    public class GetAllTodosFromBacklogQuery : IRequest<GetAllTodosFromBacklogResponse> { }

    public class GetAllTodosFromBacklogResponse
    {
        public TodoViewModel[] Todos { get; set; }
    }

    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public TodoStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? Deadline { get; set; }
    }

    public class GetAllTodosFromBacklogHandler : IRequestHandler<GetAllTodosFromBacklogQuery, GetAllTodosFromBacklogResponse>
    {
        private readonly TuduDbContext _dbContext;

        public GetAllTodosFromBacklogHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetAllTodosFromBacklogResponse> Handle(GetAllTodosFromBacklogQuery request, CancellationToken cancellationToken)
        {
            var todoViewModels = _dbContext
                .Todos
                .Select(todo => new TodoViewModel()
                {
                    Id = todo.Id,
                    Name = todo.Name,
                    Description = todo.Description,
                    Status = todo.Status,
                    CreatedDate = todo.CreatedDate,
                    Deadline = todo.Deadline
                })
                .ToArray();

            return new GetAllTodosFromBacklogResponse()
            {
                Todos = todoViewModels
            };
        }
    }
}
