/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        Parent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Parent_Id)
                .Index(t => t.Parent_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        SKU = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockCount = c.Int(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        Featured = c.Boolean(nullable: false),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.SKU)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.DeliveryProviders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryTerms = c.String(),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentProviders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ClassName = c.String(),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShopOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InternalDeliveryNumber = c.String(),
                        DeliveryNumber = c.String(),
                        Notes = c.String(),
                        InternalNotes = c.String(),
                        Customer_Id = c.String(maxLength: 128),
                        DeliveryProvider_Id = c.Int(),
                        PaymentProvider_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Customer_Id)
                .ForeignKey("dbo.DeliveryProviders", t => t.DeliveryProvider_Id)
                .ForeignKey("dbo.PaymentProviders", t => t.PaymentProvider_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.DeliveryProvider_Id)
                .Index(t => t.PaymentProvider_Id);
            
            CreateTable(
                "dbo.OrderHistoryLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        ChangedAt = c.DateTime(nullable: false),
                        Description = c.String(),
                        Snapshot = c.String(),
                        TransactionId = c.String(),
                        ChangedBy_Id = c.String(maxLength: 128),
                        ShopOrder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ChangedBy_Id)
                .ForeignKey("dbo.ShopOrders", t => t.ShopOrder_Id)
                .Index(t => t.ChangedBy_Id)
                .Index(t => t.ShopOrder_Id);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Qty = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Product_SKU = c.String(maxLength: 128),
                        ShopOrder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_SKU)
                .ForeignKey("dbo.ShopOrders", t => t.ShopOrder_Id)
                .Index(t => t.Product_SKU)
                .Index(t => t.ShopOrder_Id);
            
            AddColumn("dbo.Files", "Category_Id", c => c.Int());
            AddColumn("dbo.Files", "Product_SKU", c => c.String(maxLength: 128));
            CreateIndex("dbo.Files", "Category_Id");
            CreateIndex("dbo.Files", "Product_SKU");
            AddForeignKey("dbo.Files", "Category_Id", "dbo.Categories", "Id");
            AddForeignKey("dbo.Files", "Product_SKU", "dbo.Products", "SKU");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopOrders", "PaymentProvider_Id", "dbo.PaymentProviders");
            DropForeignKey("dbo.OrderItems", "ShopOrder_Id", "dbo.ShopOrders");
            DropForeignKey("dbo.OrderItems", "Product_SKU", "dbo.Products");
            DropForeignKey("dbo.OrderHistoryLines", "ShopOrder_Id", "dbo.ShopOrders");
            DropForeignKey("dbo.OrderHistoryLines", "ChangedBy_Id", "dbo.Users");
            DropForeignKey("dbo.ShopOrders", "DeliveryProvider_Id", "dbo.DeliveryProviders");
            DropForeignKey("dbo.ShopOrders", "Customer_Id", "dbo.Users");
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Files", "Product_SKU", "dbo.Products");
            DropForeignKey("dbo.Categories", "Parent_Id", "dbo.Categories");
            DropForeignKey("dbo.Files", "Category_Id", "dbo.Categories");
            DropIndex("dbo.ShopOrders", new[] { "PaymentProvider_Id" });
            DropIndex("dbo.OrderItems", new[] { "ShopOrder_Id" });
            DropIndex("dbo.OrderItems", new[] { "Product_SKU" });
            DropIndex("dbo.OrderHistoryLines", new[] { "ShopOrder_Id" });
            DropIndex("dbo.OrderHistoryLines", new[] { "ChangedBy_Id" });
            DropIndex("dbo.ShopOrders", new[] { "DeliveryProvider_Id" });
            DropIndex("dbo.ShopOrders", new[] { "Customer_Id" });
            DropIndex("dbo.Products", new[] { "Category_Id" });
            DropIndex("dbo.Files", new[] { "Product_SKU" });
            DropIndex("dbo.Categories", new[] { "Parent_Id" });
            DropIndex("dbo.Files", new[] { "Category_Id" });
            DropColumn("dbo.Files", "Product_SKU");
            DropColumn("dbo.Files", "Category_Id");
            DropTable("dbo.OrderItems");
            DropTable("dbo.OrderHistoryLines");
            DropTable("dbo.ShopOrders");
            DropTable("dbo.PaymentProviders");
            DropTable("dbo.DeliveryProviders");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
