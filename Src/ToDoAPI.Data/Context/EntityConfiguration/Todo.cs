using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.EntityConfiguration;

public class TodoEntityConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todo");

        builder
            .Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(e => e.Completed)
            .HasColumnName("completed")
            .HasDefaultValueSql("false");

        builder
            .Property(e => e.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()");

        builder
            .Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder
            .Property(e => e.DeletedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("deleted_at");

        builder
            .Property(e => e.Description)
            .HasColumnName("description");

        builder
            .Property(e => e.InGroup)
            .HasColumnName("in_group");

        builder
            .Property(e => e.LimitDate)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("limit_date");

        builder
            .Property(e => e.Title)
            .HasColumnName("title");

        builder
            .HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.Todos)
            .HasForeignKey(d => d.CreatedBy)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("todo_created_by_fkey");

        builder
            .HasOne(d => d.InGroupNavigation)
            .WithMany(p => p.Todos)
            .HasForeignKey(d => d.InGroup)
            .HasConstraintName("todo_in_group_fkey");
    }
}