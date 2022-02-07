using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Content.Server.Database
{
    public sealed class PostgresServerDbContext : ServerDbContext
    {
        static PostgresServerDbContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<PostgresServerRoleBan> RoleBan { get; set; } = null!;
        public DbSet<PostgresServerRoleUnban> RoleUnban { get; set; } = null!;

        public PostgresServerDbContext(DbContextOptions<PostgresServerDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.ReplaceService<IRelationalTypeMappingSource, CustomNpgsqlTypeMappingSource>();

            ((IDbContextOptionsBuilderInfrastructure) options).AddOrUpdateExtension(new SnakeCaseExtension());

            options.ConfigureWarnings(x =>
            {
                x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning);
#if DEBUG
                // for tests
                x.Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning);
#endif
            });

#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ReSharper disable once CommentTypo
            // ReSharper disable once StringLiteralTypo
            // Enforce that an address cannot be IPv6-mapped IPv4.
            // So that IPv4 addresses are consistent between separate-socket and dual-stack socket modes.
            modelBuilder.Entity<ServerBan>()
                .HasCheckConstraint("AddressNotIPv6MappedIPv4", "NOT inet '::ffff:0.0.0.0/96' >>= address");

            // ReSharper disable once StringLiteralTypo
            modelBuilder.Entity<Player>()
                .HasCheckConstraint("LastSeenAddressNotIPv6MappedIPv4",
                    "NOT inet '::ffff:0.0.0.0/96' >>= last_seen_address");

            modelBuilder.Entity<ConnectionLog>()
                .HasCheckConstraint("AddressNotIPv6MappedIPv4",
                    "NOT inet '::ffff:0.0.0.0/96' >>= address");

            foreach(var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach(var property in entity.GetProperties())
                {
                    if (property.FieldInfo?.FieldType == typeof(DateTime) || property.FieldInfo?.FieldType == typeof(DateTime?))
                        property.SetColumnType("timestamp with time zone");
                }
            }
        }
    }

    #region Job Bans
    [Table("server_role_ban")]
    public class PostgresServerRoleBan
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        [Column(TypeName = "inet")] public (IPAddress, int)? Address { get; set; }
        public byte[]? HWId { get; set; }

        public DateTime BanTime { get; set; }

        public DateTime? ExpirationTime { get; set; }

        public string Reason { get; set; } = null!;
        public Guid? BanningAdmin { get; set; }

        public PostgresServerRoleUnban? Unban { get; set; }

        public string RoleId { get; set; } = null!;
    }

    [Table("server_role_unban")]
    public class PostgresServerRoleUnban
    {
        [Column("unban_id")] public int Id { get; set; }

        public int BanId { get; set; }
        public PostgresServerRoleBan Ban { get; set; } = null!;

        public Guid? UnbanningAdmin { get; set; }

        public DateTime UnbanTime { get; set; }
    }
    #endregion
}
