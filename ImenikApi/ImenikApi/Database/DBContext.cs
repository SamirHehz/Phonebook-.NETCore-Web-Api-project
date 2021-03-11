using Imenik.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImenikApi.Database
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Kontakt>()
                    .Property(p => p.Ime)
                    .HasColumnType("nvarchar(50)");
            modelBuilder.Entity<Kontakt>()
            .Property(p => p.Prezime)
            .HasColumnType("nvarchar(50)");

        }
        public DbSet<Grad> Gradovi { get; set; }
        public DbSet<Drzava> Drzave { get; set; }
        public DbSet<Kontakt> Kontakti { get; set; }
    }
}
