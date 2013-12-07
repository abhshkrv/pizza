namespace HQServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchResponseDetails",
                c => new
                    {
                        batchResponseID = c.Int(nullable: false),
                        barcode = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.batchResponseID, t.barcode });
            
            CreateTable(
                "dbo.BatchResponses",
                c => new
                    {
                        batchResponseID = c.Int(nullable: false, identity: true),
                        requestID = c.Int(nullable: false),
                        outletID = c.Int(nullable: false),
                        timestamp = c.DateTime(nullable: false),
                        comments = c.String(),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.batchResponseID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        categoryID = c.Int(nullable: false, identity: true),
                        categoryName = c.String(),
                    })
                .PrimaryKey(t => t.categoryID);
            
            CreateTable(
                "dbo.Manufacturers",
                c => new
                    {
                        manufacturerID = c.Int(nullable: false, identity: true),
                        manufacturerName = c.String(),
                    })
                .PrimaryKey(t => t.manufacturerID);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        memberID = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        address = c.String(),
                        email = c.String(),
                        hpNumber = c.Int(nullable: false),
                        password = c.String(),
                        points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.memberID);
            
            CreateTable(
                "dbo.OutletInventories",
                c => new
                    {
                        outletID = c.Int(nullable: false),
                        barcode = c.String(nullable: false, maxLength: 128),
                        sellingPrice = c.Double(nullable: false),
                        currentStock = c.Int(nullable: false),
                        minimumStock = c.Int(nullable: false),
                        discountPercentage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.outletID, t.barcode });
            
            CreateTable(
                "dbo.Outlets",
                c => new
                    {
                        outletID = c.Int(nullable: false, identity: true),
                        owner = c.String(),
                        address = c.String(),
                    })
                .PrimaryKey(t => t.outletID);
            
            CreateTable(
                "dbo.OutletTransactionDetails",
                c => new
                    {
                        transactionSummaryID = c.Int(nullable: false),
                        outletID = c.Int(nullable: false),
                        barcode = c.String(nullable: false, maxLength: 128),
                        unitSold = c.Int(nullable: false),
                        cost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.transactionSummaryID, t.outletID, t.barcode });
            
            CreateTable(
                "dbo.OutletTransactions",
                c => new
                    {
                        transactionSummaryID = c.Int(nullable: false),
                        outletID = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.transactionSummaryID, t.outletID });
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        productID = c.Int(nullable: false, identity: true),
                        productName = c.String(),
                        barcode = c.String(),
                        categoryID = c.Int(nullable: false),
                        manufacturerID = c.Int(nullable: false),
                        costPrice = c.Double(nullable: false),
                        maxPrice = c.Double(nullable: false),
                        currentStock = c.Int(nullable: false),
                        minimumStock = c.Int(nullable: false),
                        bundleUnit = c.Int(nullable: false),
                        discountPercentage = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.productID);
            
            CreateTable(
                "dbo.OnlineTransactionDetails",
                c => new
                    {
                        transactionID = c.Int(nullable: false),
                        barcode = c.String(nullable: false, maxLength: 128),
                        shopID = c.Int(nullable: false),
                        unitSold = c.Int(nullable: false),
                        totalCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.transactionID, t.barcode });
            
            CreateTable(
                "dbo.OnlineTransactions",
                c => new
                    {
                        transactionID = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false),
                        userKey = c.String(),
                        totalTransactionCost = c.Double(nullable: false),
                        shippingAddress = c.String(),
                    })
                .PrimaryKey(t => t.transactionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OnlineTransactions");
            DropTable("dbo.OnlineTransactionDetails");
            DropTable("dbo.Products");
            DropTable("dbo.OutletTransactions");
            DropTable("dbo.OutletTransactionDetails");
            DropTable("dbo.Outlets");
            DropTable("dbo.OutletInventories");
            DropTable("dbo.Members");
            DropTable("dbo.Manufacturers");
            DropTable("dbo.Categories");
            DropTable("dbo.BatchResponses");
            DropTable("dbo.BatchResponseDetails");
        }
    }
}
