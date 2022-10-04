using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.EntityConfiguration;

public class GroupEntityConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("group");

        builder
            .Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(e => e.Admin)
            .HasColumnName("admin");

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
            .Property(e => e.Name)
            .HasColumnName("name");

        builder
            .HasOne(d => d.AdminNavigation)
            .WithMany(p => p.GroupAdminNavigations)
            .HasForeignKey(d => d.Admin)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("group_admin_fkey");

        builder
            .HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.GroupCreatedByNavigations)
            .HasForeignKey(d => d.CreatedBy)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("group_created_by_fkey");
    }
}