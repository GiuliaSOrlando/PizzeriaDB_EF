using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzeriaDB_EF.Models
{
    public class Carrello
    {
        public DettagliOrdine DettaglioOrdine { get; set; }
        public string Nome { get; set; }

    }
}