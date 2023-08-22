using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace lb4.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Exemption> Exemption { get; set; }
        public virtual DbSet<Hostel> Hostel { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Student> Student { get; set; }

        public string Writefunction(int p_hostel_id, int p_room_number) => throw new NotSupportedException();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
 //               optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));User ID=system;Password=2001Dek07;Persist Security Info=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exemption>(entity =>
            {
                entity.ToTable("EXEMPTION");

                entity.Property(e => e.ExemptionId)
                    .HasColumnName("EXEMPTION_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Discount).HasColumnName("DISCOUNT");
            });

            modelBuilder.Entity<Hostel>(entity =>
            {
                entity.ToTable("HOSTEL");

                entity.Property(e => e.HostelId)
                    .HasColumnName("HOSTEL_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.HostelNumber)
                    .HasColumnName("HOSTEL_NUMBER")
                    .HasColumnType("NUMBER(5)");

                entity.Property(e => e.Manager)
                    .IsRequired()
                    .HasColumnName("MANAGER")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.University)
                    .IsRequired()
                    .HasColumnName("UNIVERSITY")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("ROOM");

                entity.Property(e => e.RoomId)
                    .HasColumnName("ROOM_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Flat)
                    .HasColumnName("FLAT")
                    .HasColumnType("NUMBER(5)");

                entity.Property(e => e.HostelId).HasColumnName("HOSTEL_ID");

                entity.Property(e => e.NumberOfBeds)
                    .HasColumnName("NUMBER_OF_BEDS")
                    .HasColumnType("NUMBER(5)");

                entity.Property(e => e.Price)
                    .HasColumnName("PRICE")
                    .HasColumnType("NUMBER(7,2)");

                entity.Property(e => e.RoomNumber).HasColumnName("ROOM_NUMBER");

                entity.Property(e => e.Type)
                    .HasColumnName("TYPE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Hostel)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.HostelId)
                    .HasConstraintName("FK_HOSTEL_ROOM");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("STUDENT");

                entity.Property(e => e.StudentId)
                    .HasColumnName("STUDENT_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CheckInDate)
                    .HasColumnName("CHECK_IN_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("DATE_OF_BIRTH")
                    .HasColumnType("DATE");

                entity.Property(e => e.ExemptionId).HasColumnName("EXEMPTION_ID");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("GENDER")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.PassportId)
                    .IsRequired()
                    .HasColumnName("PASSPORT_ID")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RoomId).HasColumnName("ROOM_ID");

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasColumnName("STUDENT_NAME")
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.HasOne(d => d.Exemption)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.ExemptionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_EXEMPTION_STUDENT");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ROOM_STUDENT");
            });


            modelBuilder.HasSequence("LOGMNR_DIDS$");

            modelBuilder.HasSequence("LOGMNR_EVOLVE_SEQ$");

            modelBuilder.HasSequence("LOGMNR_SEQ$");

            modelBuilder.HasSequence("LOGMNR_UIDS$");

            modelBuilder.HasSequence("MVIEW$_ADVSEQ_GENERIC");

            modelBuilder.HasSequence("MVIEW$_ADVSEQ_ID");

            modelBuilder.HasSequence("ROLLING_EVENT_SEQ$");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
