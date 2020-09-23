﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using dytsenayasar.Context;

namespace dytsenayasar.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200923140123_init_7")]
    partial class init_7
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name_en")
                        .IsRequired()
                        .HasColumnName("name_en")
                        .HasColumnType("varchar(64)");

                    b.Property<string>("Name_tr")
                        .IsRequired()
                        .HasColumnName("name_tr")
                        .HasColumnType("varchar(64)");

                    b.HasKey("ID");

                    b.ToTable("category");
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.Content", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("create_time")
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("CreatorId")
                        .HasColumnName("creator_id")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid?>("File")
                        .HasColumnName("file")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("Image")
                        .HasColumnName("image")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("varchar(128)");

                    b.Property<DateTime>("ValidityDate")
                        .HasColumnName("validity_date")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ID");

                    b.HasIndex("CreatorId");

                    b.ToTable("content");
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.ContentCategory", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<int>("CategoryId")
                        .HasColumnName("category_id")
                        .HasColumnType("integer");

                    b.Property<Guid>("ContentId")
                        .HasColumnName("content_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("create_time")
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("ID");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ContentId");

                    b.ToTable("content_category");
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.User", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<Guid?>("Image")
                        .HasColumnType("uuid");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("PersonalId")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<int>("UserType")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.UserClient", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("ClientId")
                        .HasColumnName("clientid")
                        .HasColumnType("text");

                    b.Property<byte[]>("ClientIdHash")
                        .HasColumnName("clientid_hash")
                        .HasColumnType("bytea");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("create_time")
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("ClientIdHash")
                        .HasAnnotation("Npgsql:IndexMethod", "hash");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("user_client");
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.UserContent", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("ContentId")
                        .HasColumnName("content_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("create_time")
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("DeliveryType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("delivery_type")
                        .HasColumnType("varchar(16)")
                        .HasDefaultValue("Optional");

                    b.Property<Guid>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ValidityDate")
                        .HasColumnName("validity_date")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ID");

                    b.HasIndex("ContentId");

                    b.HasIndex("DeliveryType")
                        .HasAnnotation("Npgsql:IndexMethod", "hash");

                    b.HasIndex("UserId");

                    b.ToTable("user_content");
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.UserForm", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("AllergyFoods")
                        .HasColumnType("text");

                    b.Property<string>("Anemia")
                        .HasColumnType("text");

                    b.Property<string>("BadHabits")
                        .HasColumnType("text");

                    b.Property<string>("BestFoods")
                        .HasColumnType("text");

                    b.Property<string>("BreakfastFoods")
                        .HasColumnType("text");

                    b.Property<string>("BreakfastTime")
                        .HasColumnType("text");

                    b.Property<string>("Breastfeed")
                        .HasColumnType("text");

                    b.Property<string>("CallTimes")
                        .HasColumnType("text");

                    b.Property<string>("CardiovascularDiseases")
                        .HasColumnType("text");

                    b.Property<int>("Chest")
                        .HasColumnType("integer");

                    b.Property<string>("ContinuousDrugs")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("create_time")
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Diabetes")
                        .HasColumnType("text");

                    b.Property<int>("DietType")
                        .HasColumnType("integer");

                    b.Property<string>("DinnerFoods")
                        .HasColumnType("text");

                    b.Property<string>("DinnerTime")
                        .HasColumnType("text");

                    b.Property<string>("Drugs")
                        .HasColumnType("text");

                    b.Property<string>("Family")
                        .HasColumnType("text");

                    b.Property<string>("FavoriteBreakfastFoods")
                        .HasColumnType("text");

                    b.Property<string>("FavoriteFruits")
                        .HasColumnType("text");

                    b.Property<string>("FavoriteMeatFoods")
                        .HasColumnType("text");

                    b.Property<string>("FavoriteVegetablesFoods")
                        .HasColumnType("text");

                    b.Property<string>("FoodsUntilSleep")
                        .HasColumnType("text");

                    b.Property<string>("Hospital")
                        .HasColumnType("text");

                    b.Property<bool?>("IsOrderRegl")
                        .HasColumnType("boolean");

                    b.Property<bool?>("IsRegl")
                        .HasColumnType("boolean");

                    b.Property<string>("Job")
                        .HasColumnType("text");

                    b.Property<int>("Length")
                        .HasColumnType("integer");

                    b.Property<string>("LunchFoods")
                        .HasColumnType("text");

                    b.Property<string>("LunchTime")
                        .HasColumnType("text");

                    b.Property<string>("LungInfection")
                        .HasColumnType("text");

                    b.Property<float>("MaxWeight")
                        .HasColumnType("real");

                    b.Property<string>("Methods")
                        .HasColumnType("text");

                    b.Property<float>("MinWeight")
                        .HasColumnType("real");

                    b.Property<string>("NoteToDietitian")
                        .HasColumnType("text");

                    b.Property<string>("Operation")
                        .HasColumnType("text");

                    b.Property<string>("OralDiseases")
                        .HasColumnType("text");

                    b.Property<string>("Parturition")
                        .HasColumnType("text");

                    b.Property<string>("Phone2")
                        .HasColumnType("text");

                    b.Property<string>("References")
                        .HasColumnType("text");

                    b.Property<string>("SleepPatterns")
                        .HasColumnType("text");

                    b.Property<string>("SleepTime")
                        .HasColumnType("text");

                    b.Property<string>("Sports")
                        .HasColumnType("text");

                    b.Property<string>("StomachAndIntestineDiseases")
                        .HasColumnType("text");

                    b.Property<string>("ThyroidDiseases")
                        .HasColumnType("text");

                    b.Property<string>("ToiletFrequency")
                        .HasColumnType("text");

                    b.Property<string>("UrinaryInfection")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("Waist")
                        .HasColumnType("integer");

                    b.Property<string>("WakeUpTime")
                        .HasColumnType("text");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("user_form");
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.UserRequest", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("create_time")
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("RequestType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("request_type")
                        .HasColumnType("varchar(16)")
                        .HasDefaultValue("PasswordReset");

                    b.Property<string>("Token")
                        .HasColumnName("token")
                        .HasColumnType("varchar(64)");

                    b.Property<Guid>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ValidityDate")
                        .HasColumnName("validity_date")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ID");

                    b.HasIndex("RequestType")
                        .HasAnnotation("Npgsql:IndexMethod", "hash");

                    b.HasIndex("UserId");

                    b.ToTable("user_request");
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.Content", b =>
                {
                    b.HasOne("dytsenayasar.DataAccess.Entities.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.ContentCategory", b =>
                {
                    b.HasOne("dytsenayasar.DataAccess.Entities.Category", "Category")
                        .WithMany("ContentCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dytsenayasar.DataAccess.Entities.Content", "Content")
                        .WithMany("ContentCategories")
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.UserClient", b =>
                {
                    b.HasOne("dytsenayasar.DataAccess.Entities.User", "User")
                        .WithOne("Client")
                        .HasForeignKey("dytsenayasar.DataAccess.Entities.UserClient", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.UserContent", b =>
                {
                    b.HasOne("dytsenayasar.DataAccess.Entities.Content", "Content")
                        .WithMany("UserContents")
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dytsenayasar.DataAccess.Entities.User", "User")
                        .WithMany("UserContents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.UserForm", b =>
                {
                    b.HasOne("dytsenayasar.DataAccess.Entities.User", "User")
                        .WithOne("Form")
                        .HasForeignKey("dytsenayasar.DataAccess.Entities.UserForm", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dytsenayasar.DataAccess.Entities.UserRequest", b =>
                {
                    b.HasOne("dytsenayasar.DataAccess.Entities.User", "User")
                        .WithMany("Requests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
