using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tudu.Domain.Entities;
using Tudu.Domain.Enums;

namespace Tudu.Infrastructure.Data.EntityConfigurations
{
    internal class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder
                .ToContainer("Todos")
                .HasKey(t => t.Id);

            builder
                .Property(t => t.Status)
                .HasConversion<EnumToStringConverter<TodoStatus>>();
        }
    }
}
