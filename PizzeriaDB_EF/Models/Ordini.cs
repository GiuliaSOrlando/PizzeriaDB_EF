namespace PizzeriaDB_EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordini")]
    public partial class Ordini
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordini()
        {
            DettagliOrdine = new HashSet<DettagliOrdine>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdOrdine { get; set; }

        public int? IdUtente { get; set; }
        public DateTime? DataOrdine { get; set; }

        [StringLength(50)]
        public string StatoOrdine { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettagliOrdine> DettagliOrdine { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}
