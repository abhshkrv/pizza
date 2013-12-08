namespace HQServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _decimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OutletInventories", "sellingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.OutletTransactionDetails", "cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "costPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "maxPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "discountPercentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.OnlineTransactionDetails", "totalCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.OnlineTransactions", "totalTransactionCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OnlineTransactions", "totalTransactionCost", c => c.Double(nullable: false));
            AlterColumn("dbo.OnlineTransactionDetails", "totalCost", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "discountPercentage", c => c.Single(nullable: false));
            AlterColumn("dbo.Products", "maxPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "costPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.OutletTransactionDetails", "cost", c => c.Double(nullable: false));
            AlterColumn("dbo.OutletInventories", "sellingPrice", c => c.Double(nullable: false));
        }
    }
}
