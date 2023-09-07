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

        public DbSet<InsuranceCompanies> InsuranceCompanies { get; set; }

        public DbSet<InsuranceCo_Doctor> InsuranceCo_Doctor { get; set; } // 白色字体对应控制器中的'_context.InsuranceCo_Doctor'

        public DbSet<LoginHistory> LoginHistory { get; set; }

        public DbSet<DoctorInfo> DoctorInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
            // 定义UserName Email唯一
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.PrimaryEmail).IsUnique();

            modelBuilder.Entity<Title>();
            modelBuilder.Entity<Title>().HasIndex(u => u.TitleName).IsUnique();

            modelBuilder.Entity<Specialty>();
            modelBuilder.Entity<Specialty>().HasIndex(u => u.SpecialtyName).IsUnique();

            modelBuilder.Entity<InsuranceCompanies>();
            modelBuilder.Entity<InsuranceCompanies>().HasIndex(u => u.InsuranceCoName).IsUnique();


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
            modelBuilder.Entity<InsuranceCo_Doctor>()
                .HasKey(di => di.Id); // 将主键配置为 Id

            modelBuilder.Entity<InsuranceCo_Doctor>()
                .HasOne(di => di.Doctor)
                .WithMany()
                .HasForeignKey(di => di.DoctorId);

            modelBuilder.Entity<InsuranceCo_Doctor>()
                .HasOne(di => di.InsuranceCompanies)
                .WithMany()
                .HasForeignKey(di => di.InsuranceCoId);

            // 唯一性索引
            modelBuilder.Entity<InsuranceCo_Doctor>()
            .HasIndex(di => new { di.DoctorId, di.InsuranceCoId, di.Deleted })
            .IsUnique();

        }

    }

}