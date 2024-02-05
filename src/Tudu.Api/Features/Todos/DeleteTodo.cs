using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tudu.Api.Shared.Endpoint;
using Tudu.Infrastructure.Data;

namespace Tudu.Api.Features.Todos
{
    public class DeleteTodoEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapDelete("/todo/{todoId:guid}", (IMediator mediator, [FromRoute] Guid todoId) =>
                {
                    return mediator.Send(new DeleteTodoCommand(todoId));
                })
                .WithName(nameof(DeleteTodoEndpoint));
            return routeBuilder;
        }
    }

    public class DeleteTodoCommand : IRequest<DeleteTodoResponse>
    {
        public DeleteTodoCommand(Guid todoId)
        {
            TodoId = todoId;
        }

        public Guid TodoId { get; set; }
    }

    public class DeleteTodoResponse
    {

    }

    public class DeleteTodoValidator : AbstractValidator<DeleteTodoCommand>
    {
        public DeleteTodoValidator()
        {
            RuleFor(model => model.TodoId)
                .NotEqual(Guid.Empty);
        }
    }

    public class DeleteTodoHandler : IRequestHandler<DeleteTodoCommand, DeleteTodoResponse>
    {
        private readonly TuduDbContext _dbContext;

        public DeleteTodoHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeleteTodoResponse> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _dbContext.Todos.SingleOrDefaultAsync(todo => todo.Id == request.TodoId);
            if (todo == null)
                throw new ValidationException($"There is no todo with id: {request.TodoId}");

            _dbContext.Todos.Remove(todo);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteTodoResponse();
        }
    }
}
