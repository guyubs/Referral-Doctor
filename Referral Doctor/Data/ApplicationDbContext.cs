using Microsoft.EntityFrameworkCore;

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

            // 定义UserName Email唯一
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();


            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Title)
                .WithMany()
                .HasForeignKey(d => d.TitleId);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialty)
                .WithMany()
                .HasForeignKey(d => d.SpecialtyId);


            // 添加DoctorAddress体的配置
            modelBuilder.Entity<DoctorAddress>()
                .HasKey(di => di.Id); // 将主键配置为 Id

            modelBuilder.Entity<DoctorAddress>()
                .HasOne(di => di.Doctor)
                .WithMany()
                .HasForeignKey(di => di.DoctorId);

            modelBuilder.Entity<DoctorAddress>()
                .HasOne(di => di.Address)
                .WithMany()
                .HasForeignKey(di => di.AddressId);

            // 唯一性索引
            modelBuilder.Entity<DoctorAddress>()
            .HasIndex(di => new { di.DoctorId, di.AddressId, di.Deleted })
            .IsUnique();


            // 添加DoctorInsurance体的配置
            modelBuilder.Entity<DoctorInsurance>()
                .HasKey(di => di.Id); // 将主键配置为 Id

            modelBuilder.Entity<DoctorInsurance>()
                .HasOne(di => di.Doctor)
                .WithMany()
                .HasForeignKey(di => di.DoctorId);

            modelBuilder.Entity<DoctorInsurance>()
                .HasOne(di => di.Insurance)
                .WithMany()
                .HasForeignKey(di => di.InsuranceId);

            // 唯一性索引
            modelBuilder.Entity<DoctorInsurance>()
            .HasIndex(di => new { di.DoctorId, di.InsuranceId, di.Deleted })
            .IsUnique();

        }

    }

}