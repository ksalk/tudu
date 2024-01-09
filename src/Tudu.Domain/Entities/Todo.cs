using Tudu.Domain.Enums;

namespace Tudu.Domain.Entities
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public TodoStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
