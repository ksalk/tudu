using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tudu.Api.Features.Todos.ViewModels;
using Tudu.Api.Shared.Endpoint;
using Tudu.Infrastructure.Data;

namespace Tudu.Api.Features.TodosForToday
{
    // TODO: make it change in bulk
    public class AssignTodoToDateEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapPut("/todo/{todoId:guid}/assign-date", (IMediator mediator, [FromRoute] Guid todoId, [FromQuery] DateOnly date) =>
                {
                    return mediator.Send(new AssignTodoToDateCommand(todoId, date));
                })
                .WithName(nameof(AssignTodoToDateEndpoint));
            return routeBuilder;
        }
    }

    public class AssignTodoToDateCommand : IRequest<AssignTodoToDateResponse> 
    { 
        public AssignTodoToDateCommand(Guid todoId, DateOnly date)
        {
            TodoId = todoId;
            Date = date;
        }

        public Guid TodoId { get; private set; }
        public DateOnly Date { get; private set; }
    }

     public class AssignTodoToDateValidator : AbstractValidator<AssignTodoToDateCommand>
    {
        public AssignTodoToDateValidator()
        {
            RuleFor(model => model.Date)
                .NotNull();
        }
    }

    public class AssignTodoToDateResponse
    {
        public TodoViewModel Todo { get; set; }
    }

    public class AssignTodoToDateHandler : IRequestHandler<AssignTodoToDateCommand, AssignTodoToDateResponse>
    {
        private readonly TuduDbContext _dbContext;

        public AssignTodoToDateHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AssignTodoToDateResponse> Handle(AssignTodoToDateCommand request, CancellationToken cancellationToken)
        {
            var todo = await _dbContext
                .Todos
                .FirstOrDefaultAsync(todo => todo.Id == request.TodoId);

            if(todo == null)
                throw new ValidationException($"There is no todo with id: {request.TodoId}");

            todo.AssignToDate(request.Date);

            return new AssignTodoToDateResponse()
            {
                Todo = TodoViewModel.From(todo)
            };
        }
    }
}
