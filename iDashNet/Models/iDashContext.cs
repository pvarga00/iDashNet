using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using iDashNet.Models;
using Microsoft.Extensions.Options;

namespace iDashNet.Models
{
    public partial class iDashContext : DbContext
    {
        // Unable to generate entity type for table 'dbo.AppData'. Please see the warning messages.
        private string _connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=5PLJWF2\LOCALSQLSERVER;Database=iDash;Trusted_Connection=True;integrated security=True");
            optionsBuilder.UseSqlServer(this._connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppData>(entity =>
            {
                entity.HasKey(e => e.ID).HasName("ID");
                entity.Property(e => e.ID).UseSqlServerIdentityColumn();
                entity.ToTable("AppData", "dbo");
                entity.ForSqlServerToTable("AppData");
            });
        }

        public iDashContext(DbContextOptions<iDashContext> options, IOptions<MySettings> settings) : base(options)
        {
            this._connectionString = settings.Value.DbContextConnectionString;
        }

        public iDashContext() { }

        public DbSet<iDashNet.Models.AppData> AppData { get; set; }
    }
}