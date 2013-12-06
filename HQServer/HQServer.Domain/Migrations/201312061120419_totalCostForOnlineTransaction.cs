namespace HQServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class totalCostForOnlineTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OnlineTransactions", "totalTransactionCost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OnlineTransactions", "totalTransactionCost");
        }
    }
}
