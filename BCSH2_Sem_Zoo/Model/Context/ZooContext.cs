using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BCSH2_Sem_Zoo.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace BCSH2_Sem_Zoo.Model.Context
{
    public class ZooContext : DbContext
    {
        public DbSet<Animal> Animal { get; set; }
        public DbSet<Caretaker> Caretaker { get; set; }
        public DbSet<Show> Show { get; set; }
        public DbSet<BreedingNeed> BreedingNeed { get; set; }
        public DbSet<BreedingNeedType> BreedingNeedType { get; set; }
        public DbSet<HistoryAnimalBreedingNeed> HistoryAnimalBreedingNeed { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=zoo.db");
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
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && (t.FullName?.StartsWith("BCSH2_Sem_Zoo.Model.Entity.") ?? false)).All(t =>
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
