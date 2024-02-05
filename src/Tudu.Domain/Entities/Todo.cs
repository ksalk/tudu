using Tudu.Domain.Enums;

namespace Tudu.Domain.Entities
{
    // TODO: use more Domain Driven approach
    public class Todo
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public TodoStatus Status { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? Deadline { get; private set; }
        public DateOnly? AssignedDate { get; private set; }

        private Todo(
            string name,
            string description,
            DateTime? deadline,
            DateOnly? assignedDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Status = TodoStatus.NotStarted;
            CreatedDate = DateTime.UtcNow;
            Deadline = deadline;
            AssignedDate = assignedDate;
        }
        
        public static Todo Create(
            string name,
            string description,
            DateTime? deadline,
            DateOnly? assignedDate
        )
        {
            return new Todo(name, description, deadline, assignedDate);
        }

        public Todo UpdateDetails(
            string name,
            string description,
            DateTime? deadline
        )
        {
            Name = name;
            Description = description;
            Deadline = deadline;

            return this;
        }

        public Todo UpdateStatus(TodoStatus status)
        {
            Status = status;

            return this;
        }

        public Todo AssignToDate(DateOnly? date)
        {
            AssignedDate = date;

            return this;
        }

         public Todo UnassignFromDate()
        {
            AssignedDate = null;

            return this;
        }

        public Todo Update(
            string name,
            string description,
            DateTime? deadline,
            DateOnly? assignedDate,
            TodoStatus status
        )
        {
            UpdateDetails(name, description, deadline);
            UpdateStatus(status);
            AssignToDate(assignedDate);

            return this;
        }
    }
}
