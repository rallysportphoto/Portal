/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        EventTag_Id = c.String(maxLength: 128),
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.EventTag_Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.EventTag_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Confirmed = c.Boolean(nullable: false),
                        Redimmed = c.Boolean(nullable: false),
                        StartNumber = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        Car_Id = c.Int(),
                        CoDriver_Id = c.String(maxLength: 128),
                        Driver_Id = c.String(maxLength: 128),
                        EventTag_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.Car_Id)
                .ForeignKey("dbo.Users", t => t.CoDriver_Id)
                .ForeignKey("dbo.Users", t => t.Driver_Id)
                .ForeignKey("dbo.Tags", t => t.EventTag_Id)
                .Index(t => t.Car_Id)
                .Index(t => t.CoDriver_Id)
                .Index(t => t.Driver_Id)
                .Index(t => t.EventTag_Id);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mark = c.String(),
                        Model = c.String(),
                        RegistrationNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "EventTag_Id", "dbo.Tags");
            DropForeignKey("dbo.Groups", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Driver_Id", "dbo.Users");
            DropForeignKey("dbo.Orders", "CoDriver_Id", "dbo.Users");
            DropForeignKey("dbo.Orders", "Car_Id", "dbo.Cars");
            DropForeignKey("dbo.Groups", "EventTag_Id", "dbo.Tags");
            DropIndex("dbo.Orders", new[] { "EventTag_Id" });
            DropIndex("dbo.Groups", new[] { "Order_Id" });
            DropIndex("dbo.Orders", new[] { "Driver_Id" });
            DropIndex("dbo.Orders", new[] { "CoDriver_Id" });
            DropIndex("dbo.Orders", new[] { "Car_Id" });
            DropIndex("dbo.Groups", new[] { "EventTag_Id" });
            DropTable("dbo.Cars");
            DropTable("dbo.Orders");
            DropTable("dbo.Groups");
        }
    }
}
