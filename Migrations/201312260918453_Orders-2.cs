/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders2 : DbMigration
    {
        public override void Up()
        {            
            DropForeignKey("dbo.Groups", "SubEvent_Id", "dbo.SubEvents");
            DropForeignKey("dbo.Groups", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Groups", new[] { "SubEvent_Id" });
            DropIndex("dbo.Groups", new[] { "Order_Id" });
            DropColumn("dbo.Groups", "SubEvent_Id");
            DropColumn("dbo.Groups", "Order_Id");
            
            CreateTable(
                "dbo.GroupSubEvents",
                c => new
                {
                    Group_Id = c.Int(nullable: false),
                    SubEvent_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Group_Id, t.SubEvent_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.SubEvents", t => t.SubEvent_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.SubEvent_Id);

            CreateTable(
                "dbo.GroupOrders",
                c => new
                {
                    Group_Id = c.Int(nullable: false),
                    Order_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Group_Id, t.Order_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.Order_Id);            
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        City = c.String(),
                        License = c.String(),
                        ASN = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "Team_Id", c => c.Int());
            CreateIndex("dbo.Orders", "Team_Id");                        
        }
        
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
