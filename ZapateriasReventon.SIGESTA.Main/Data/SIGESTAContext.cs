using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace ZapateriasReventon.SIGESTA.Main.Data
{   public class SIGESTAContext : DbContext
    {
        public SIGESTAContext() : base("name=SIGESTADB") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<SIGESTAContext>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Lecturas>().ToTable("Lecturas");
        }

        public virtual DbSet<Lecturas> Lecturas { get; set; }
    }
}