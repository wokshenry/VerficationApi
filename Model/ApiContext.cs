using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace VerficationApi.Model
{
    public class ApiContext: DbContext
    {
        private string DbPath = String.Empty;
        public ApiContext(string dbName)
        {
            try
            {
                SQLitePCL.Batteries_V2.Init();
                var _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
                this.DbPath = _dbPath;
                //this.Database.EnsureDeleted();
                this.Database.EnsureCreated();
            }
            catch (Exception ex)
            {

            }

        }

        public DbSet<TelecomData>? TelecomData { get; set; }
        public DbSet<RequestData>? RequestData { get; set; }
        public DbSet<ResponseData>? ResponseData { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={this.DbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<TelecomData>(entity =>
            {
                entity.HasKey(e => e.TelecomId);
            });
            modelBuilder.Entity<RequestData>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<ResponseData>(entity =>
            {
                entity.HasKey(e => e.ResponseId);
            });
        }
    }
}
