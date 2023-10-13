namespace PizzeriaDB_EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    namespace PizzeriaDB_EF.Models
    {
        public class DettaglioIngredienti
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int IdDettaglioIngredienti { get; set; }

            [Required]
            public int IdDettaglioOrdine { get; set; }
            [Required]
            public int IdIngredienteExtra { get; set; }

            [ForeignKey("IdDettaglioOrdine")]
            public virtual DettagliOrdine DettaglioOrdine { get; set; }

            [ForeignKey("IdIngredienteExtra")]
            public virtual IngredientiExtra IngredienteExtra { get; set; }

        }
    }
}