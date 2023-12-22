using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dentistry_API.Models
{
    public partial class DentistryContext : DbContext
    {
        public DentistryContext()
        {
        }

        public DentistryContext(DbContextOptions<DentistryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Clinic> Clinics { get; set; } = null!;
        public virtual DbSet<DateAppointment> DateAppointments { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<History> Histories { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<PriceAppointment> PriceAppointments { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=DESKTOP-DNTIAC2\\CAT;Initial Catalog=Dentistry;User ID=sa;Password=12345");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.IdAppointment);

                entity.ToTable("Appointment");

                entity.Property(e => e.IdAppointment).HasColumnName("ID_Appointment");

                entity.Property(e => e.Complaint)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DateAppointmentId).HasColumnName("Date_Appointment_ID");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");

                entity.Property(e => e.Number)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PatientId).HasColumnName("Patient_ID");

                entity.Property(e => e.ServiceId).HasColumnName("Service_ID");

                entity.Property(e => e.Treatment)
                    .HasMaxLength(200)
                    .IsUnicode(false);

               
            });

            modelBuilder.Entity<Clinic>(entity =>
            {
                entity.HasKey(e => e.IdClinic);

                entity.ToTable("Clinic");

                entity.Property(e => e.IdClinic).HasColumnName("ID_Clinic");

                entity.Property(e => e.AddresClinic)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Addres_Clinic");
            });

            modelBuilder.Entity<DateAppointment>(entity =>
            {
                entity.HasKey(e => e.IdDateAppointment);

                entity.ToTable("Date_Appointment");

                entity.Property(e => e.IdDateAppointment).HasColumnName("ID_Date_Appointment");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.IdEmployee);

                entity.ToTable("Employee");

                entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");

                entity.Property(e => e.ClinicId).HasColumnName("Clinic_ID");

                entity.Property(e => e.EmailEmployee)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Email_Employee");

                entity.Property(e => e.FirstNameEmployee)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("First_Name_Employee");

                entity.Property(e => e.MiddleEmployee)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Middle_Employee");

                entity.Property(e => e.PasswordEmployee)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Password_Employee");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.SurnameEmployee)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Surname_Employee");

               
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("History");

                entity.Property(e => e.Дата).HasColumnType("date");

                entity.Property(e => e.Жалоба)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Лечение)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Пациент)
                    .HasMaxLength(152)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.IdPatient);

                entity.ToTable("Patient");

                entity.Property(e => e.IdPatient).HasColumnName("ID_Patient");

                entity.Property(e => e.Age)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailPatient)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Email_Patient");

                entity.Property(e => e.FirstNamePatient)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("First_Name_Patient");

                entity.Property(e => e.MiddlePatient)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Middle_Patient");

                entity.Property(e => e.PasswordPatient)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Password_Patient");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.Sex)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SurnamePatient)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Surname_Patient");

               
            });

            modelBuilder.Entity<PriceAppointment>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Price_Appointment");

                entity.Property(e => e.Дата).HasColumnType("date");

                entity.Property(e => e.НомерЗаписи)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Номер записи");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole);

                entity.ToTable("Role");

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.NameRole)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_Role");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.IdService);

                entity.ToTable("Service");

                entity.Property(e => e.IdService).HasColumnName("ID_Service");

                entity.Property(e => e.NameService)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_Service");

                entity.Property(e => e.PriceService).HasColumnName("Price_Service");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
