namespace HQServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class batch1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchDispatchDetails",
                c => new
                    {
                        batchDispatchID = c.Int(nullable: false),
                        barcode = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.batchDispatchID, t.barcode });
            
            CreateTable(
                "dbo.BatchDispatches",
                c => new
                    {
                        batchDispatchID = c.Int(nullable: false, identity: true),
                        outletID = c.Int(nullable: false),
                        timestamp = c.DateTime(nullable: false),
                        comments = c.String(),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.batchDispatchID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BatchDispatches");
            DropTable("dbo.BatchDispatchDetails");
        }
    }
}
