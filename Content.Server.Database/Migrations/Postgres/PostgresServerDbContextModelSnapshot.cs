﻿// <auto-generated />
using System;
using System.Net;
using System.Text.Json;
using Content.Server.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
{
    [DbContext(typeof(PostgresServerDbContext))]
    partial class PostgresServerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Content.Server.Database.Admin", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<int?>("AdminRankId")
                        .HasColumnType("integer")
                        .HasColumnName("admin_rank_id");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("UserId")
                        .HasName("PK_admin");

                    b.HasIndex("AdminRankId")
                        .HasDatabaseName("IX_admin_admin_rank_id");

                    b.ToTable("admin", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.AdminFlag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("admin_flag_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uuid")
                        .HasColumnName("admin_id");

                    b.Property<string>("Flag")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("flag");

                    b.Property<bool>("Negative")
                        .HasColumnType("boolean")
                        .HasColumnName("negative");

                    b.HasKey("Id")
                        .HasName("PK_admin_flag");

                    b.HasIndex("AdminId")
                        .HasDatabaseName("IX_admin_flag_admin_id");

                    b.HasIndex("Flag", "AdminId")
                        .IsUnique();

                    b.ToTable("admin_flag", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.AdminLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("admin_log_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("RoundId")
                        .HasColumnType("integer")
                        .HasColumnName("round_id");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<short>("Impact")
                        .HasColumnType("smallint")
                        .HasColumnName("impact");

                    b.Property<JsonDocument>("Json")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("json");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.HasKey("Id", "RoundId")
                        .HasName("PK_admin_log");

                    b.HasIndex("RoundId")
                        .HasDatabaseName("IX_admin_log_round_id");

                    b.HasIndex("Type")
                        .HasDatabaseName("IX_admin_log_type");

                    b.ToTable("admin_log", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.AdminLogEntity", b =>
                {
                    b.Property<int>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("uid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Uid"));

                    b.Property<int?>("AdminLogId")
                        .HasColumnType("integer")
                        .HasColumnName("admin_log_id");

                    b.Property<int?>("AdminLogRoundId")
                        .HasColumnType("integer")
                        .HasColumnName("admin_log_round_id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Uid")
                        .HasName("PK_admin_log_entity");

                    b.HasIndex("AdminLogId", "AdminLogRoundId")
                        .HasDatabaseName("IX_admin_log_entity_admin_log_id_admin_log_round_id");

                    b.ToTable("admin_log_entity", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.AdminLogPlayer", b =>
                {
                    b.Property<Guid>("PlayerUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("player_user_id");

                    b.Property<int>("LogId")
                        .HasColumnType("integer")
                        .HasColumnName("log_id");

                    b.Property<int>("RoundId")
                        .HasColumnType("integer")
                        .HasColumnName("round_id");

                    b.HasKey("PlayerUserId", "LogId", "RoundId")
                        .HasName("PK_admin_log_player");

                    b.HasIndex("LogId", "RoundId");

                    b.ToTable("admin_log_player", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.AdminRank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("admin_rank_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PK_admin_rank");

                    b.ToTable("admin_rank", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.AdminRankFlag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("admin_rank_flag_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AdminRankId")
                        .HasColumnType("integer")
                        .HasColumnName("admin_rank_id");

                    b.Property<string>("Flag")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("flag");

                    b.HasKey("Id")
                        .HasName("PK_admin_rank_flag");

                    b.HasIndex("AdminRankId")
                        .HasDatabaseName("IX_admin_rank_flag_admin_rank_id");

                    b.HasIndex("Flag", "AdminRankId")
                        .IsUnique();

                    b.ToTable("admin_rank_flag", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.Antag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("antag_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AntagName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("antag_name");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer")
                        .HasColumnName("profile_id");

                    b.HasKey("Id")
                        .HasName("PK_antag");

                    b.HasIndex("ProfileId", "AntagName")
                        .IsUnique();

                    b.ToTable("antag", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.AssignedUserId", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("assigned_user_id_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("PK_assigned_user_id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("assigned_user_id", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.ConnectionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("connection_log_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<IPAddress>("Address")
                        .IsRequired()
                        .HasColumnType("inet")
                        .HasColumnName("address");

                    b.Property<byte?>("Denied")
                        .HasColumnType("smallint")
                        .HasColumnName("denied");

                    b.Property<byte[]>("HWId")
                        .HasColumnType("bytea")
                        .HasColumnName("hwid");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("time");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("PK_connection_log");

                    b.HasIndex("UserId");

                    b.ToTable("connection_log", (string)null);

                    b.HasCheckConstraint("AddressNotIPv6MappedIPv4", "NOT inet '::ffff:0.0.0.0/96' >>= address");
                });

            modelBuilder.Entity("Content.Server.Database.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("job_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("job_name");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("priority");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer")
                        .HasColumnName("profile_id");

                    b.HasKey("Id")
                        .HasName("PK_job");

                    b.HasIndex("ProfileId")
                        .HasDatabaseName("IX_job_profile_id");

                    b.HasIndex("ProfileId", "JobName")
                        .IsUnique();

                    b.HasIndex(new[] { "ProfileId" }, "IX_job_one_high_priority")
                        .IsUnique()
                        .HasFilter("priority = 3");

                    b.ToTable("job", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("player_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FirstSeenTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("first_seen_time");

                    b.Property<IPAddress>("LastSeenAddress")
                        .IsRequired()
                        .HasColumnType("inet")
                        .HasColumnName("last_seen_address");

                    b.Property<byte[]>("LastSeenHWId")
                        .HasColumnType("bytea")
                        .HasColumnName("last_seen_hwid");

                    b.Property<DateTime>("LastSeenTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_seen_time");

                    b.Property<string>("LastSeenUserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_seen_user_name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK_player");

                    b.HasAlternateKey("UserId")
                        .HasName("ak_player_user_id");

                    b.HasIndex("LastSeenUserName");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("player", (string)null);

                    b.HasCheckConstraint("LastSeenAddressNotIPv6MappedIPv4", "NOT inet '::ffff:0.0.0.0/96' >>= last_seen_address");
                });

            modelBuilder.Entity("Content.Server.Database.ServerRoleBan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("server_role_ban_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<ValueTuple<IPAddress, int>?>("Address")
                        .HasColumnType("inet")
                        .HasColumnName("address");

                    b.Property<DateTime>("BanTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ban_time");

                    b.Property<Guid?>("BanningAdmin")
                        .HasColumnType("uuid")
                        .HasColumnName("banning_admin");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_time");

                    b.Property<byte[]>("HWId")
                        .HasColumnType("bytea")
                        .HasColumnName("hwid");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_id");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK_server_role_ban");

                    b.ToTable("server_role_ban", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.ServerRoleUnban", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("unban_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BanId")
                        .HasColumnType("integer")
                        .HasColumnName("ban_id");

                    b.Property<DateTime>("UnbanTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("unban_time");

                    b.Property<Guid?>("UnbanningAdmin")
                        .HasColumnType("uuid")
                        .HasColumnName("unbanning_admin");

                    b.HasKey("Id")
                        .HasName("PK_server_role_unban");

                    b.HasIndex("BanId")
                        .IsUnique();

                    b.ToTable("server_role_unban", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.Preference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("preference_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminOOCColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("admin_ooc_color");

                    b.Property<int>("SelectedCharacterSlot")
                        .HasColumnType("integer")
                        .HasColumnName("selected_character_slot");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK_preference");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("preference", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("profile_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("integer")
                        .HasColumnName("age");

                    b.Property<string>("Backpack")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("backpack");

                    b.Property<string>("CharacterName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("char_name");

                    b.Property<string>("Clothing")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("clothing");

                    b.Property<string>("EyeColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("eye_color");

                    b.Property<string>("FacialHairColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("facial_hair_color");

                    b.Property<string>("FacialHairName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("facial_hair_name");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("gender");

                    b.Property<string>("HairColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hair_color");

                    b.Property<string>("HairName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hair_name");

                    b.Property<int>("PreferenceId")
                        .HasColumnType("integer")
                        .HasColumnName("preference_id");

                    b.Property<int>("PreferenceUnavailable")
                        .HasColumnType("integer")
                        .HasColumnName("pref_unavailable");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sex");

                    b.Property<string>("SkinColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("skin_color");

                    b.Property<int>("Slot")
                        .HasColumnType("integer")
                        .HasColumnName("slot");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("species");

                    b.HasKey("Id")
                        .HasName("PK_profile");

                    b.HasIndex("PreferenceId")
                        .HasDatabaseName("IX_profile_preference_id");

                    b.HasIndex("Slot", "PreferenceId")
                        .IsUnique();

                    b.ToTable("profile", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.Round", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("round_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.HasKey("Id")
                        .HasName("PK_round");

                    b.ToTable("round", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.ServerBan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("server_ban_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<ValueTuple<IPAddress, int>?>("Address")
                        .HasColumnType("inet")
                        .HasColumnName("address");

                    b.Property<DateTime>("BanTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ban_time");

                    b.Property<Guid?>("BanningAdmin")
                        .HasColumnType("uuid")
                        .HasColumnName("banning_admin");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_time");

                    b.Property<byte[]>("HWId")
                        .HasColumnType("bytea")
                        .HasColumnName("hwid");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PK_server_ban");

                    b.HasIndex("Address");

                    b.HasIndex("UserId");

                    b.ToTable("server_ban", (string)null);

                    b.HasCheckConstraint("AddressNotIPv6MappedIPv4", "NOT inet '::ffff:0.0.0.0/96' >>= address");

                    b.HasCheckConstraint("HaveEitherAddressOrUserIdOrHWId", "address IS NOT NULL OR user_id IS NOT NULL OR hwid IS NOT NULL");
                });

            modelBuilder.Entity("Content.Server.Database.ServerBanHit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("server_ban_hit_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BanId")
                        .HasColumnType("integer")
                        .HasColumnName("ban_id");

                    b.Property<int>("ConnectionId")
                        .HasColumnType("integer")
                        .HasColumnName("connection_id");

                    b.HasKey("Id")
                        .HasName("PK_server_ban_hit");

                    b.HasIndex("BanId")
                        .HasDatabaseName("IX_server_ban_hit_ban_id");

                    b.HasIndex("ConnectionId")
                        .HasDatabaseName("IX_server_ban_hit_connection_id");

                    b.ToTable("server_ban_hit", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.ServerUnban", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("unban_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BanId")
                        .HasColumnType("integer")
                        .HasColumnName("ban_id");

                    b.Property<DateTime>("UnbanTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("unban_time");

                    b.Property<Guid?>("UnbanningAdmin")
                        .HasColumnType("uuid")
                        .HasColumnName("unbanning_admin");

                    b.HasKey("Id")
                        .HasName("PK_server_unban");

                    b.HasIndex("BanId")
                        .IsUnique();

                    b.ToTable("server_unban", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.Whitelist", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("UserId")
                        .HasName("PK_whitelist");

                    b.ToTable("whitelist", (string)null);
                });

            modelBuilder.Entity("PlayerRound", b =>
                {
                    b.Property<int>("PlayersId")
                        .HasColumnType("integer")
                        .HasColumnName("players_id");

                    b.Property<int>("RoundsId")
                        .HasColumnType("integer")
                        .HasColumnName("rounds_id");

                    b.HasKey("PlayersId", "RoundsId")
                        .HasName("PK_player_round");

                    b.HasIndex("RoundsId")
                        .HasDatabaseName("IX_player_round_rounds_id");

                    b.ToTable("player_round", (string)null);
                });

            modelBuilder.Entity("Content.Server.Database.Admin", b =>
                {
                    b.HasOne("Content.Server.Database.AdminRank", "AdminRank")
                        .WithMany("Admins")
                        .HasForeignKey("AdminRankId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_admin_admin_rank_admin_rank_id");

                    b.Navigation("AdminRank");
                });

            modelBuilder.Entity("Content.Server.Database.AdminFlag", b =>
                {
                    b.HasOne("Content.Server.Database.Admin", "Admin")
                        .WithMany("Flags")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_admin_flag_admin_admin_id");

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("Content.Server.Database.AdminLog", b =>
                {
                    b.HasOne("Content.Server.Database.Round", "Round")
                        .WithMany("AdminLogs")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_admin_log_round_round_id");

                    b.Navigation("Round");
                });

            modelBuilder.Entity("Content.Server.Database.AdminLogEntity", b =>
                {
                    b.HasOne("Content.Server.Database.AdminLog", null)
                        .WithMany("Entities")
                        .HasForeignKey("AdminLogId", "AdminLogRoundId")
                        .HasConstraintName("FK_admin_log_entity_admin_log_admin_log_id_admin_log_round_id");
                });

            modelBuilder.Entity("Content.Server.Database.AdminLogPlayer", b =>
                {
                    b.HasOne("Content.Server.Database.Player", "Player")
                        .WithMany("AdminLogs")
                        .HasForeignKey("PlayerUserId")
                        .HasPrincipalKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_admin_log_player_player_player_user_id");

                    b.HasOne("Content.Server.Database.AdminLog", "Log")
                        .WithMany("Players")
                        .HasForeignKey("LogId", "RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_admin_log_player_admin_log_log_id_round_id");

                    b.Navigation("Log");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Content.Server.Database.AdminRankFlag", b =>
                {
                    b.HasOne("Content.Server.Database.AdminRank", "Rank")
                        .WithMany("Flags")
                        .HasForeignKey("AdminRankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_admin_rank_flag_admin_rank_admin_rank_id");

                    b.Navigation("Rank");
                });

            modelBuilder.Entity("Content.Server.Database.Antag", b =>
                {
                    b.HasOne("Content.Server.Database.Profile", "Profile")
                        .WithMany("Antags")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_antag_profile_profile_id");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Content.Server.Database.Job", b =>
                {
                    b.HasOne("Content.Server.Database.Profile", "Profile")
                        .WithMany("Jobs")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_job_profile_profile_id");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Content.Server.Database.ServerRoleUnban", b =>
                {
                    b.HasOne("Content.Server.Database.ServerRoleBan", "Ban")
                        .WithOne("Unban")
                        .HasForeignKey("Content.Server.Database.ServerRoleUnban", "BanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_server_ban_hit_connection_log_connection_id");

                    b.Navigation("Ban");

                    b.Navigation("Connection");
                });

            modelBuilder.Entity("PlayerRound", b =>
                {
                    b.HasOne("Content.Server.Database.Player", null)
                        .WithMany()
                        .HasForeignKey("PlayersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_player_round_player_players_id");

                    b.HasOne("Content.Server.Database.Round", null)
                        .WithMany()
                        .HasForeignKey("RoundsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_player_round_round_rounds_id");
                });

            modelBuilder.Entity("Content.Server.Database.Admin", b =>
                {
                    b.Navigation("Flags");
                });

            modelBuilder.Entity("Content.Server.Database.AdminLog", b =>
                {
                    b.Navigation("Entities");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("Content.Server.Database.AdminRank", b =>
                {
                    b.Navigation("Admins");

                    b.Navigation("Flags");
                });

            modelBuilder.Entity("Content.Server.Database.ConnectionLog", b =>
                {
                    b.Navigation("BanHits");
                });

            modelBuilder.Entity("Content.Server.Database.Player", b =>
                {
                    b.Navigation("AdminLogs");
                });

            modelBuilder.Entity("Content.Server.Database.PostgresServerBan", b =>
                {
                    b.Navigation("Unban");
                });

            modelBuilder.Entity("Content.Server.Database.ServerRoleBan", b =>
                {
                    b.Navigation("Unban");
                });

            modelBuilder.Entity("Content.Server.Database.Preference", b =>
                {
                    b.Navigation("Profiles");
                });

            modelBuilder.Entity("Content.Server.Database.Profile", b =>
                {
                    b.Navigation("Antags");

                    b.Navigation("Jobs");
                });

            modelBuilder.Entity("Content.Server.Database.Round", b =>
                {
                    b.Navigation("AdminLogs");
                });

            modelBuilder.Entity("Content.Server.Database.ServerBan", b =>
                {
                    b.Navigation("BanHits");

                    b.Navigation("Unban");
                });
#pragma warning restore 612, 618
        }
    }
}
