/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gallery : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(),
                        Author = c.String(),
                        Name = c.String(),
                        Code = c.String(),
                        PublicationTime = c.DateTime(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                        Tag_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .Index(t => t.Tag_Id);
            
            AddColumn("dbo.Files", "Gallery_Id", c => c.Int());
            CreateIndex("dbo.Files", "Gallery_Id");
            AddForeignKey("dbo.Files", "Gallery_Id", "dbo.Galleries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Galleries", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Files", "Gallery_Id", "dbo.Galleries");
            DropIndex("dbo.Galleries", new[] { "Tag_Id" });
            DropIndex("dbo.Files", new[] { "Gallery_Id" });
            DropColumn("dbo.Files", "Gallery_Id");
            DropTable("dbo.Galleries");
        }
    }
}
