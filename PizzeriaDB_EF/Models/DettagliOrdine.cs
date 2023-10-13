namespace PizzeriaDB_EF.Models
{
    using global::PizzeriaDB_EF.Models.PizzeriaDB_EF.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("DettagliOrdine")]
    public partial class DettagliOrdine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDettaglio { get; set; }

        public int IdOrdine { get; set; }

        public int? IdProdotto { get; set; }

        public int? Quantita { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? PrezzoTotale { get; set; }

        [Required]
        [StringLength(50)]
        public string IndirizzoSpedizione { get; set; }

        [StringLength(50)]
        public string NoteAggiuntive { get; set; }

        public virtual Ordini Ordini { get; set; }

        public virtual Prodotti Prodotti { get; set; }

        public virtual ICollection<DettaglioIngredienti> DettaglioIngredienti { get; set; }


    }
}
