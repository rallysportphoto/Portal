/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DomainFilterProto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Domain = c.String(),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SiteTags",
                c => new
                    {
                        Site_Id = c.Int(nullable: false),
                        Tag_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Site_Id, t.Tag_Id })
                .ForeignKey("dbo.Sites", t => t.Site_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .Index(t => t.Site_Id)
                .Index(t => t.Tag_Id);
            
            DropColumn("dbo.Events", "Scope");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "Scope", c => c.String());
            DropForeignKey("dbo.SiteTags", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.SiteTags", "Site_Id", "dbo.Sites");
            DropIndex("dbo.SiteTags", new[] { "Tag_Id" });
            DropIndex("dbo.SiteTags", new[] { "Site_Id" });
            DropTable("dbo.SiteTags");
            DropTable("dbo.Sites");
        }
    }
}
