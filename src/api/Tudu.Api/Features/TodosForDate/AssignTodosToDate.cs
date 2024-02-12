using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tudu.Api.Shared.Endpoint;
using Tudu.Infrastructure.Data;

namespace Tudu.Api.Features.TodosForDate
{
    public class AssignTodosToDateEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapPut("/todo/assign-date", (IMediator mediator, [FromBody] AssignTodosToDateCommand command) =>
                {
                    return mediator.Send(command);
                })
                .WithName(nameof(AssignTodosToDateEndpoint));
            return routeBuilder;
        }
    }

    public class AssignTodosToDateCommand : IRequest<AssignTodosToDateResponse> 
    { 
        public AssignTodosToDateCommand(Guid[] todoIds, DateOnly date)
        {
            TodoIds = todoIds;
            Date = date;
        }

        public Guid[] TodoIds { get; private set; }
        public DateOnly Date { get; private set; }
    }

    public class AssignTodosToDateValidator : AbstractValidator<AssignTodosToDateCommand>
    {
        public AssignTodosToDateValidator()
        {
            RuleFor(model => model.TodoIds)
                .NotNull()
                .NotEmpty();
            
            RuleFor(model => model.Date)
                .NotNull();
        }
    }

    public class AssignTodosToDateResponse { }

    public class AssignTodosToDateHandler : IRequestHandler<AssignTodosToDateCommand, AssignTodosToDateResponse>
    {
        private readonly TuduDbContext _dbContext;

        public AssignTodosToDateHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AssignTodosToDateResponse> Handle(AssignTodosToDateCommand request, CancellationToken cancellationToken)
        {
            var todos = _dbContext
                .Todos
                .Where(todo => request.TodoIds.Contains(todo.Id));

            foreach (var todo in todos)
            {
                todo.AssignToDate(request.Date);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new AssignTodosToDateResponse();
        }
    }
}
