namespace PizzeriaDB_EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DettaglioIngredientis",
                c => new
                    {
                        IdDettaglioIngredienti = c.Int(nullable: false, identity: true),
                        IdDettaglioOrdine = c.Int(nullable: false),
                        IdIngredienteExtra = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDettaglioIngredienti)
                .ForeignKey("dbo.DettagliOrdine", t => t.IdDettaglioOrdine, cascadeDelete: true)
                .ForeignKey("dbo.IngredientiExtras", t => t.IdIngredienteExtra, cascadeDelete: true)
                .Index(t => t.IdDettaglioOrdine)
                .Index(t => t.IdIngredienteExtra);
            
            CreateTable(
                "dbo.DettagliOrdine",
                c => new
                    {
                        IdDettaglio = c.Int(nullable: false, identity: true),
                        IdOrdine = c.Int(),
                        IdProdotto = c.Int(),
                        Quantita = c.Int(),
                        PrezzoTotale = c.Decimal(precision: 18, scale: 2),
                        IndirizzoSpedizione = c.String(nullable: false, maxLength: 50),
                        NoteAggiuntive = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IdDettaglio)
                .ForeignKey("dbo.Ordini", t => t.IdOrdine)
                .ForeignKey("dbo.Prodotti", t => t.IdProdotto)
                .Index(t => t.IdOrdine)
                .Index(t => t.IdProdotto);
            
            CreateTable(
                "dbo.Ordini",
                c => new
                    {
                        IdOrdine = c.Int(nullable: false, identity: true),
                        IdUtente = c.Int(),
                        DataOrdine = c.DateTime(),
                        StatoOrdine = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IdOrdine)
                .ForeignKey("dbo.Utenti", t => t.IdUtente)
                .Index(t => t.IdUtente);
            
            CreateTable(
                "dbo.Utenti",
                c => new
                    {
                        IdUtente = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        Ruolo = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IdUtente);
            
            CreateTable(
                "dbo.Prodotti",
                c => new
                    {
                        IdProdotto = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 50),
                        Foto = c.String(maxLength: 50),
                        Prezzo = c.Decimal(precision: 18, scale: 2),
                        TempoConsegna = c.Int(),
                        Ingredienti = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IdProdotto);
            
            CreateTable(
                "dbo.IngredientiExtras",
                c => new
                    {
                        IdIngredientiExtra = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Prezzo = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IdIngredientiExtra);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DettaglioIngredientis", "IdIngredienteExtra", "dbo.IngredientiExtras");
            DropForeignKey("dbo.DettagliOrdine", "IdProdotto", "dbo.Prodotti");
            DropForeignKey("dbo.Ordini", "IdUtente", "dbo.Utenti");
            DropForeignKey("dbo.DettagliOrdine", "IdOrdine", "dbo.Ordini");
            DropForeignKey("dbo.DettaglioIngredientis", "IdDettaglioOrdine", "dbo.DettagliOrdine");
            DropIndex("dbo.Ordini", new[] { "IdUtente" });
            DropIndex("dbo.DettagliOrdine", new[] { "IdProdotto" });
            DropIndex("dbo.DettagliOrdine", new[] { "IdOrdine" });
            DropIndex("dbo.DettaglioIngredientis", new[] { "IdIngredienteExtra" });
            DropIndex("dbo.DettaglioIngredientis", new[] { "IdDettaglioOrdine" });
            DropTable("dbo.IngredientiExtras");
            DropTable("dbo.Prodotti");
            DropTable("dbo.Utenti");
            DropTable("dbo.Ordini");
            DropTable("dbo.DettagliOrdine");
            DropTable("dbo.DettaglioIngredientis");
        }
    }
}
