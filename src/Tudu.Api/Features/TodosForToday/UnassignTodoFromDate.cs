using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tudu.Api.Features.Todos.ViewModels;
using Tudu.Api.Shared.Endpoint;
using Tudu.Infrastructure.Data;

namespace Tudu.Api.Features.TodosForToday
{
    public class UnassignTodoFromDateEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapPut("/todo/{todoId:guid}/unassign-date", (IMediator mediator, [FromRoute] Guid todoId) =>
                {
                    return mediator.Send(new UnassignTodoFromDateCommand(todoId));
                })
                .WithName(nameof(UnassignTodoFromDateEndpoint));
            return routeBuilder;
        }
    }

    public class UnassignTodoFromDateCommand : IRequest<UnassignTodoFromDateResponse> 
    { 
        public UnassignTodoFromDateCommand(Guid todoId)
        {
            TodoId = todoId;
        }

        public Guid TodoId { get; private set; }
    }

    public class UnassignTodoFromDateResponse
    {
        public TodoViewModel Todo { get; set; }
    }

    public class UnassignTodoFromDateHandler : IRequestHandler<UnassignTodoFromDateCommand, UnassignTodoFromDateResponse>
    {
        private readonly TuduDbContext _dbContext;

        public UnassignTodoFromDateHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UnassignTodoFromDateResponse> Handle(UnassignTodoFromDateCommand request, CancellationToken cancellationToken)
        {
            var todo = await _dbContext
                .Todos
                .FirstOrDefaultAsync(todo => todo.Id == request.TodoId);

            if(todo == null)
                throw new ValidationException($"There is no todo with id: {request.TodoId}");

            todo.UnassignFromDate();

            return new UnassignTodoFromDateResponse()
            {
                Todo = TodoViewModel.From(todo)
            };
        }
    }
}
