using PizzeriaDB_EF.Models.PizzeriaDB_EF.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PizzeriaDB_EF.Models
{
    public partial class ModelDBContext : DbContext
    {
        public ModelDBContext()
            : base("name=ModelDBContext")
        {
        }

        public virtual DbSet<DettagliOrdine> DettagliOrdine { get; set; }
        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<Prodotti> Prodotti { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }
        public virtual DbSet<IngredientiExtra> IngredientiExtra { get; set; }
        public virtual DbSet<DettaglioIngredienti> DettaglioIngredienti { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
