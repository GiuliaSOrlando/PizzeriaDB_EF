namespace PizzeriaDB_EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prodotti")]
    public partial class Prodotti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prodotti()
        {
            DettagliOrdine = new HashSet<DettagliOrdine>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProdotto { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string Foto { get; set; }

        public decimal? Prezzo { get; set; }
        [DisplayName("Tempo di consegna")]
        public int? TempoConsegna { get; set; }

        [StringLength(50)]
        public string Ingredienti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettagliOrdine> DettagliOrdine { get; set; }
    }
}
