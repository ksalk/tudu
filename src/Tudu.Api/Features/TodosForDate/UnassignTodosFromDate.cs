using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tudu.Api.Shared.Endpoint;
using Tudu.Infrastructure.Data;

namespace Tudu.Api.Features.TodosForDate
{
    public class UnassignTodosFromDateEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapPut("/todo/unassign-date", (IMediator mediator, [FromBody] UnassignTodosFromDateCommand command) =>
                {
                    return mediator.Send(command);
                })
                .WithName(nameof(UnassignTodosFromDateEndpoint));
            return routeBuilder;
        }
    }

    public class UnassignTodosFromDateCommand : IRequest<UnassignTodosFromDateResponse> 
    { 
        public UnassignTodosFromDateCommand(Guid[] todoIds)
        {
            TodoIds = todoIds;
        }

        public Guid[] TodoIds { get; private set; }
    }

    public class UnassignTodosFromDateValidator : AbstractValidator<UnassignTodosFromDateCommand>
    {
        public UnassignTodosFromDateValidator()
        {
            RuleFor(model => model.TodoIds)
                .NotNull()
                .NotEmpty();
        }
    }

    public class UnassignTodosFromDateResponse { }

    public class UnassignTodosFromDateHandler : IRequestHandler<UnassignTodosFromDateCommand, UnassignTodosFromDateResponse>
    {
        private readonly TuduDbContext _dbContext;

        public UnassignTodosFromDateHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UnassignTodosFromDateResponse> Handle(UnassignTodosFromDateCommand request, CancellationToken cancellationToken)
        {
            var todos = _dbContext
                .Todos
                .Where(todo => request.TodoIds.Contains(todo.Id));

            foreach (var todo in todos)
            {
                todo.UnassignFromDate();
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UnassignTodosFromDateResponse();
        }
    }
}
