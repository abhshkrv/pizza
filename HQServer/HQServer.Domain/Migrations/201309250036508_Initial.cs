namespace HQServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchResponses",
                c => new
                    {
                        requestID = c.Int(nullable: false, identity: true),
                        outletID = c.Int(nullable: false),
                        timestamp = c.DateTime(nullable: false),
                        comments = c.String(),
                    })
                .PrimaryKey(t => t.requestID);
            
            CreateTable(
                "dbo.BatchResponseDetails",
                c => new
                    {
                        requestID = c.Int(nullable: false),
                        barcode = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.requestID, t.barcode });
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        categoryID = c.Int(nullable: false, identity: true),
                        categoryName = c.String(),
                    })
                .PrimaryKey(t => t.categoryID);
            
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
                "dbo.Manufacturers",
                c => new
                    {
                        manufacturerID = c.Int(nullable: false, identity: true),
                        manufacturerName = c.String(),
                    })
                .PrimaryKey(t => t.manufacturerID);
            
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
                "dbo.OutletInventories",
                c => new
                    {
                        outletID = c.Int(nullable: false),
                        barcode = c.Int(nullable: false),
                        sellingPrice = c.Single(nullable: false),
                        currentStock = c.Int(nullable: false),
                        minimumStock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.outletID, t.barcode });
            
            CreateTable(
                "dbo.OutletTransactions",
                c => new
                    {
                        transactionID = c.String(nullable: false, maxLength: 128),
                        outletID = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.transactionID, t.outletID });
            
            CreateTable(
                "dbo.OutletTransactionDetails",
                c => new
                    {
                        transactionID = c.Int(nullable: false),
                        outletID = c.Int(nullable: false),
                        barcode = c.Int(nullable: false),
                        unitSold = c.Int(nullable: false),
                        cost = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.transactionID, t.outletID, t.barcode });
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        productID = c.Int(nullable: false, identity: true),
                        productName = c.String(),
                        barcode = c.String(),
                        categoryID = c.Int(nullable: false),
                        manufacturerID = c.Int(nullable: false),
                        costPrice = c.Single(nullable: false),
                        maxPrice = c.Single(nullable: false),
                        currentStock = c.Int(nullable: false),
                        minimumStock = c.Int(nullable: false),
                        bundleUnit = c.Int(nullable: false),
                        discountPercentage = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.productID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
            DropTable("dbo.OutletTransactionDetails");
            DropTable("dbo.OutletTransactions");
            DropTable("dbo.OutletInventories");
            DropTable("dbo.Outlets");
            DropTable("dbo.Manufacturers");
            DropTable("dbo.Members");
            DropTable("dbo.Categories");
            DropTable("dbo.BatchResponseDetails");
            DropTable("dbo.BatchResponses");
        }
    }
}
