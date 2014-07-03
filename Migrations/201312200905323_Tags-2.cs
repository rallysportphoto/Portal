/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tags2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "News_Id", "dbo.News");
            DropIndex("dbo.Tags", new[] { "News_Id" });
            DropColumn("dbo.Tags", "News_Id");

            CreateTable(
                "dbo.TagNews",
                c => new
                {
                    Tag_Id = c.String(nullable: false, maxLength: 128),
                    News_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Tag_Id, t.News_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.News", t => t.News_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.News_Id);
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "News_Id", c => c.Int());            
            CreateIndex("dbo.Tags", "News_Id");
            AddForeignKey("dbo.Tags", "News_Id", "dbo.News", "Id");


            DropForeignKey("dbo.TagNews", "News_Id", "dbo.News");
            DropForeignKey("dbo.TagNews", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagNews", new[] { "News_Id" });
            DropIndex("dbo.TagNews", new[] { "Tag_Id" });
            DropTable("dbo.TagNews");
        }
    }
}
