using System.Linq;
using System.Reflection;
using BCSH2_Sem_Zoo.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace BCSH2_Sem_Zoo.Model.Context
{
    public class ZooContext : DbContext
    {
        public DbSet<Animal> Animal { get; set; }
        public DbSet<Spieces> Spieces { get; set; }
        public DbSet<Caretaker> Caretaker { get; set; }
        public DbSet<Show> Show { get; set; }
        public DbSet<BreedingNeed> BreedingNeed { get; set; }
        public DbSet<BreedingNeedType> BreedingNeedType { get; set; }
        public DbSet<HistoryAnimalBreedingNeed> HistoryAnimalBreedingNeed { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=C:\Users\user\Desktop\zoo.db");
        }

        public void ChangeDatabasePath(string path)
        {
            Database.SetConnectionString($"Data Source={path};");
        }

        public bool IsTableEmpty<T>() where T : class
        {
            return Set<T>().Count() == 0;
        }

        public bool IsDatabaseEmpty()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && (t.Namespace == "BCSH2_Sem_Zoo.Model.Entity")).All(t =>
            {
                bool.TryParse(typeof(ZooContext).GetMethod(nameof(IsTableEmpty))?.MakeGenericMethod(new[] { t }).Invoke(this, null)?.ToString(), out bool res);
                return res;
            });
        }

        public int MaxIdOfEntity<T>() where T : class
        {
            return Set<T>()
                        .OrderByDescending(e => EF.Property<int>(e, "Id"))
                        .Select(e => EF.Property<int>(e, "Id"))
                        .FirstOrDefault();
        }
    }
}
