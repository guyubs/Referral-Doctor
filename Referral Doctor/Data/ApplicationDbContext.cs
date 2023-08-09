using Microsoft.EntityFrameworkCore;
using ReferralDoctor.Models;

namespace Referral_Doctor.Models
{

    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Title> Titles { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<DoctorAddress> DoctorAddresses { get; set; }

        public DbSet<Insurance> Insurances { get; set; }

        public DbSet<DoctorInsurance> DoctorInsurances { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>();

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Title)
                .WithMany()
                .HasForeignKey(d => d.TitleId);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialty)
                .WithMany()
                .HasForeignKey(d => d.SpecialtyId);

            modelBuilder.Entity<DoctorAddress>()
                .HasKey(da => new { da.DoctorId, da.AddressId });

            modelBuilder.Entity<DoctorAddress>()
                .HasOne(da => da.Doctor)
                .WithMany()
                .HasForeignKey(da => da.DoctorId);

            modelBuilder.Entity<DoctorAddress>()
                .HasOne(da => da.Address)
                .WithMany()
                .HasForeignKey(da => da.AddressId);

            // 添加Insurance体的配置
            modelBuilder.Entity<Insurance>();

            // 添加DoctorInsurance体的配置
            modelBuilder.Entity<DoctorInsurance>()
                .HasKey(di => new { di.DoctorId, di.InsuranceId });

            modelBuilder.Entity<DoctorInsurance>()
                .HasOne(di => di.Doctor)
                .WithMany()
                .HasForeignKey(di => di.DoctorId);

            modelBuilder.Entity<DoctorInsurance>()
                .HasOne(di => di.Insurance)
                .WithMany()
                .HasForeignKey(di => di.InsuranceId);

            // 定义UserName Email唯一
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        }

    }

}