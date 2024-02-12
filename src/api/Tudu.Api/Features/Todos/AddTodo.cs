using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tudu.Api.Features.Todos.ViewModels;
using Tudu.Api.Shared.Endpoint;
using Tudu.Domain.Entities;
using Tudu.Domain.Enums;
using Tudu.Infrastructure.Data;

namespace Tudu.Api.Features.Todos
{
    public class AddTodoEndpoint : IEndpoint
    {
        public IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapPost("/todo", (IMediator mediator, [FromBody] AddTodoCommand command) =>
                {
                    return mediator.Send(command);
                })
                .WithName(nameof(AddTodoEndpoint));
            return routeBuilder;
        }
    }

    public record AddTodoCommand : IRequest<AddTodoResponse>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateOnly? AssignedDate { get; set; }
    }

    public class AddTodoResponse
    {
        public TodoViewModel Todo { get; set; }
    }

    public class AddTodoValidator : AbstractValidator<AddTodoCommand>
    {
        public AddTodoValidator()
        {
            RuleFor(model => model.Name)
                .NotNull()
                .NotEmpty();
        }
    }

    public class AddTodoHandler : IRequestHandler<AddTodoCommand, AddTodoResponse>
    {
        private readonly TuduDbContext _dbContext;

        public AddTodoHandler(TuduDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AddTodoResponse> Handle(AddTodoCommand request, CancellationToken cancellationToken)
        {
            var newTodo = Todo.Create(request.Name, request.Description, request.Deadline, request.AssignedDate);

            await _dbContext.Todos.AddAsync(newTodo, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new AddTodoResponse()
            {
                Todo = TodoViewModel.From(newTodo)
            };
        }
    }
}
