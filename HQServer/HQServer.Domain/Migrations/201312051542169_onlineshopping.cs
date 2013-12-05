namespace HQServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class onlineshopping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OnlineTransactionDetails",
                c => new
                    {
                        transactionID = c.Int(nullable: false),
                        barcode = c.String(nullable: false, maxLength: 128),
                        shopID = c.Int(nullable: false),
                        unitSold = c.Int(nullable: false),
                        totalCost = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.transactionID, t.barcode });
            
            CreateTable(
                "dbo.OnlineTransactions",
                c => new
                    {
                        transactionID = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false),
                        cashierID = c.Int(nullable: false),
                        userKey = c.String(),
                    })
                .PrimaryKey(t => t.transactionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OnlineTransactions");
            DropTable("dbo.OnlineTransactionDetails");
        }
    }
}
