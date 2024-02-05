using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tudu.Api.Features.Todos.ViewModels;
using Tudu.Api.Shared.Endpoint;
using Tudu.Domain.Enums;
using Tudu.Infrastructure.Data;

namespace Tudu.Api.Features.Todos
{
    public class UpdateTodoEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapPut("/todo/{todoId:guid}", (IMediator mediator, [FromRoute] Guid todoId, [FromBody] UpdateTodoCommand command) =>
                {
                    command.TodoId = todoId;
                    return mediator.Send(command);
                })
                .WithName(nameof(UpdateTodoEndpoint));
            return routeBuilder;
        }
    }

    public record UpdateTodoCommand : IRequest<UpdateTodoResponse>
    {
        public Guid TodoId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public TodoStatus Status { get; set; }
        public DateOnly? AssignedDate { get; set; }
    }

    public class UpdateTodoResponse
    {
        public TodoViewModel Todo { get; set; }
    }

    public class UpdateTodoValidator : AbstractValidator<UpdateTodoCommand>
    {
        public UpdateTodoValidator()
        {
            RuleFor(model => model.TodoId)
                .NotNull();

            RuleFor(model => model.Name)
                .NotNull()
                .NotEmpty();
        }
    }

    public class UpdateTodoHandler : IRequestHandler<UpdateTodoCommand, UpdateTodoResponse>
    {
        private readonly TuduDbContext _dbContext;

        public UpdateTodoHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateTodoResponse> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == request.TodoId);
            if(todo == null)
                throw new ValidationException($"There is no todo with id: {request.TodoId}");

            todo.Update(request.Name, request.Description, request.Deadline, request.AssignedDate, request.Status);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateTodoResponse()
            {
                Todo = TodoViewModel.From(todo)
            };
        }
    }
}
