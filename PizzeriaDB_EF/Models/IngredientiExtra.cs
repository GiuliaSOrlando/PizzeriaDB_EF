using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzeriaDB_EF.Models
{
    public class IngredientiExtra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdIngredientiExtra { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Prezzo { get; set; }

    }
}