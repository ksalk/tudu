using Tudu.Domain.Entities;
using Tudu.Domain.Enums;

namespace Tudu.Api.Features.Todos.ViewModels
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public TodoStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? Deadline { get; set; }
        public DateOnly? AssignedDate { get; set; }

        private TodoViewModel(Todo todo)
        {
            Id = todo.Id;
            Name = todo.Name;
            Description = todo.Description;
            Status = todo.Status;
            CreatedDate = todo.CreatedDate;
            Deadline = todo.Deadline;
            AssignedDate = todo.AssignedDate;
        }
        
        public static TodoViewModel From(Todo todo)
        {
            return new TodoViewModel(todo);
        }
    }
}
