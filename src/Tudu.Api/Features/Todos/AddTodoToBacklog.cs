using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tudu.Api.Shared.Endpoint;
using Tudu.Domain.Entities;
using Tudu.Domain.Enums;
using Tudu.Infrastructure.Data;

namespace Tudu.Api.Features.Todos
{
    public class AddTodoToBacklogEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapPost("/todo/backlog", (IMediator mediator, [FromBody] AddTodoToBacklogCommand command) =>
                {
                    return mediator.Send(command);
                })
                .WithName(nameof(AddTodoToBacklogEndpoint));
            return routeBuilder;
        }
    }

    public class AddTodoToBacklogCommand : IRequest<AddTodoToBacklogResponse>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
    }

    public class AddTodoToBacklogResponse
    {
        public Guid Id { get; set; }
    }

    public class AddTodoToBacklogValidator
    {

    }

    public class AddTodoToBacklogHandler : IRequestHandler<AddTodoToBacklogCommand, AddTodoToBacklogResponse>
    {
        private readonly TuduDbContext _dbContext;

        public AddTodoToBacklogHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AddTodoToBacklogResponse> Handle(AddTodoToBacklogCommand request, CancellationToken cancellationToken)
        {
            var newTodo = new Todo()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Status = TodoStatus.NotStarted,
                CreatedDate = DateTime.UtcNow,
                Deadline = request.Deadline
            };

            await _dbContext.Todos.AddAsync(newTodo, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new AddTodoToBacklogResponse()
            {
                Id = newTodo.Id
            };
        }
    }
}
