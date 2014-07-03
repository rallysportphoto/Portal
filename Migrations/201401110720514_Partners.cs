/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Partners : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Partners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Desctiption = c.String(),
                        Global = c.Boolean(nullable: false),
                        Url = c.String(),
                        IsEnabled = c.Boolean(nullable: false),
                        Logo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.Logo_Id)
                .Index(t => t.Logo_Id);
            
            CreateTable(
                "dbo.PartnerTags",
                c => new
                    {
                        Partner_Id = c.Int(nullable: false),
                        Tag_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Partner_Id, t.Tag_Id })
                .ForeignKey("dbo.Partners", t => t.Partner_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .Index(t => t.Partner_Id)
                .Index(t => t.Tag_Id);
            
            AddColumn("dbo.Files", "Hidden", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartnerTags", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.PartnerTags", "Partner_Id", "dbo.Partners");
            DropForeignKey("dbo.Partners", "Logo_Id", "dbo.Files");
            DropIndex("dbo.PartnerTags", new[] { "Tag_Id" });
            DropIndex("dbo.PartnerTags", new[] { "Partner_Id" });
            DropIndex("dbo.Partners", new[] { "Logo_Id" });
            DropColumn("dbo.Files", "Hidden");
            DropTable("dbo.PartnerTags");
            DropTable("dbo.Partners");
        }
    }
}
