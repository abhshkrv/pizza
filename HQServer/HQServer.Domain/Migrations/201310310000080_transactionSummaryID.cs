namespace HQServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactionSummaryID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OutletTransactionDetails", "transactionSummaryID", c => c.Int(nullable: false));
            AddColumn("dbo.OutletTransactions", "transactionSummaryID", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.OutletTransactionDetails");
            AddPrimaryKey("dbo.OutletTransactionDetails", new[] { "transactionSummaryID", "outletID", "barcode" });
            DropPrimaryKey("dbo.OutletTransactions");
            AddPrimaryKey("dbo.OutletTransactions", new[] { "transactionSummaryID", "outletID" });
            DropColumn("dbo.OutletTransactionDetails", "transactionID");
            DropColumn("dbo.OutletTransactions", "transactionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OutletTransactions", "transactionID", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.OutletTransactionDetails", "transactionID", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.OutletTransactions");
            AddPrimaryKey("dbo.OutletTransactions", new[] { "transactionID", "outletID" });
            DropPrimaryKey("dbo.OutletTransactionDetails");
            AddPrimaryKey("dbo.OutletTransactionDetails", new[] { "transactionID", "outletID", "barcode" });
            DropColumn("dbo.OutletTransactions", "transactionSummaryID");
            DropColumn("dbo.OutletTransactionDetails", "transactionSummaryID");
        }
    }
}
