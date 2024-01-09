using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tudu.Api.Shared.Endpoint;
using Tudu.Infrastructure.Data;

namespace Tudu.Api.Features.Todos
{
    public class DeleteTodoFromBacklogEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapDelete("/todo/backlog", (IMediator mediator, [FromBody] DeleteTodoFromBacklogCommand command) =>
                {
                    return mediator.Send(command);
                })
                .WithName(nameof(DeleteTodoFromBacklogEndpoint));
            return routeBuilder;
        }
    }

    public class DeleteTodoFromBacklogCommand : IRequest<DeleteTodoFromBacklogResponse>
    {
        public Guid TodoId { get; set; }
    }

    //Change to respective classes with prefix - does not break swagger
    public class DeleteTodoFromBacklogResponse
    {

    }

    public class DeleteTodoFromBacklogValidator
    {

    }

    public class DeleteTodoFromBacklogHandler : IRequestHandler<DeleteTodoFromBacklogCommand, DeleteTodoFromBacklogResponse>
    {
        private readonly TuduDbContext _dbContext;

        public DeleteTodoFromBacklogHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeleteTodoFromBacklogResponse> Handle(DeleteTodoFromBacklogCommand request, CancellationToken cancellationToken)
        {
            var todo = await _dbContext.Todos.SingleOrDefaultAsync(todo => todo.Id == request.TodoId);
            if (todo == null)
                throw new InvalidOperationException("Nonexistent todo id");

            _dbContext.Todos.Remove(todo);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteTodoFromBacklogResponse();
        }
    }
}
