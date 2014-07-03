/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficialEvent0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Event_Id = c.Int(),
                        Map_Id = c.Int(),
                        Series_Id = c.Int(),
                        Tag_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.Files", t => t.Map_Id)
                .ForeignKey("dbo.Series", t => t.Series_Id)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.Map_Id)
                .Index(t => t.Series_Id)
                .Index(t => t.Tag_Id);
            
            AddColumn("dbo.Events", "Slug", c => c.String());
            AddColumn("dbo.Events", "History", c => c.String());
            AddColumn("dbo.Events", "Logo_Id", c => c.Int());
            CreateIndex("dbo.Events", "Logo_Id");
            AddForeignKey("dbo.Events", "Logo_Id", "dbo.Files", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubEvents", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.SubEvents", "Series_Id", "dbo.Series");
            DropForeignKey("dbo.SubEvents", "Map_Id", "dbo.Files");
            DropForeignKey("dbo.SubEvents", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Events", "Logo_Id", "dbo.Files");
            DropIndex("dbo.SubEvents", new[] { "Tag_Id" });
            DropIndex("dbo.SubEvents", new[] { "Series_Id" });
            DropIndex("dbo.SubEvents", new[] { "Map_Id" });
            DropIndex("dbo.SubEvents", new[] { "Event_Id" });
            DropIndex("dbo.Events", new[] { "Logo_Id" });
            DropColumn("dbo.Events", "Logo_Id");
            DropColumn("dbo.Events", "History");
            DropColumn("dbo.Events", "Slug");
            DropTable("dbo.SubEvents");
        }
    }
}
