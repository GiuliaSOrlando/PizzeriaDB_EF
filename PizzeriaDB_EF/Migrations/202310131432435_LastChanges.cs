namespace PizzeriaDB_EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DettagliOrdine", "IdOrdine", "dbo.Ordini");
            DropIndex("dbo.DettagliOrdine", new[] { "IdOrdine" });
            AlterColumn("dbo.DettagliOrdine", "IdOrdine", c => c.Int(nullable: false));
            CreateIndex("dbo.DettagliOrdine", "IdOrdine");
            AddForeignKey("dbo.DettagliOrdine", "IdOrdine", "dbo.Ordini", "IdOrdine", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DettagliOrdine", "IdOrdine", "dbo.Ordini");
            DropIndex("dbo.DettagliOrdine", new[] { "IdOrdine" });
            AlterColumn("dbo.DettagliOrdine", "IdOrdine", c => c.Int());
            CreateIndex("dbo.DettagliOrdine", "IdOrdine");
            AddForeignKey("dbo.DettagliOrdine", "IdOrdine", "dbo.Ordini", "IdOrdine");
        }
    }
}
