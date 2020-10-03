namespace RestaurantWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Discount",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                        DiscountType = c.Int(nullable: false),
                        FlatAmount = c.Double(),
                        Percent = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.Int(nullable: false),
                        Name = c.String(maxLength: 25),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderMenuItem",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        MenuItemID = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderID, t.MenuItemID })
                .ForeignKey("dbo.MenuItem", t => t.MenuItemID, cascadeDelete: true)
                .ForeignKey("dbo.Order", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.MenuItemID);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimePlaced = c.DateTime(nullable: false),
                        ServerName = c.String(),
                        SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreTaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tax",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        Percentage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderMenuItem", "OrderID", "dbo.Order");
            DropForeignKey("dbo.OrderMenuItem", "MenuItemID", "dbo.MenuItem");
            DropIndex("dbo.OrderMenuItem", new[] { "MenuItemID" });
            DropIndex("dbo.OrderMenuItem", new[] { "OrderID" });
            DropTable("dbo.Tax");
            DropTable("dbo.Order");
            DropTable("dbo.OrderMenuItem");
            DropTable("dbo.MenuItem");
            DropTable("dbo.Discount");
        }
    }
}
