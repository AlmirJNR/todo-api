using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(e => e.AuthServerId)
            .HasName("user_pkey");

        builder.ToTable("user");

        builder
            .Property(e => e.AuthServerId)
            .ValueGeneratedNever()
            .HasColumnName("auth_server_id");

        builder
            .Property(e => e.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()");

        builder
            .Property(e => e.DeletedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("deleted_at");

        builder
            .Property(e => e.RoleId)
            .HasColumnName("role_id");

        builder
            .HasOne(d => d.Role)
            .WithMany(p => p.Users)
            .HasForeignKey(d => d.RoleId)
            .HasConstraintName("user_role_id_fkey");

        builder
            .HasMany(d => d.Groups)
            .WithMany(p => p.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserGroup",
                l => l
                    .HasOne<Group>()
                    .WithMany()
                    .HasForeignKey("GroupId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_group_group_id_fkey"),
                r => r
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_group_user_id_fkey"),
                j =>
                {
                    j.HasKey("UserId", "GroupId").HasName("user_group_pkey");

                    j.ToTable("user_group");

                    j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");

                    j.IndexerProperty<Guid>("GroupId").HasColumnName("group_id");
                });
    }
}