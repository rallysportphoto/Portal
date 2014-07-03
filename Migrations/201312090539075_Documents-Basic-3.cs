/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentsBasic3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TagFiles",
                c => new
                    {
                        Tag_Id = c.String(nullable: false, maxLength: 128),
                        File_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.File_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Files", t => t.File_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.File_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagFiles", "File_Id", "dbo.Files");
            DropForeignKey("dbo.TagFiles", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagFiles", new[] { "File_Id" });
            DropIndex("dbo.TagFiles", new[] { "Tag_Id" });
            DropTable("dbo.TagFiles");
        }
    }
}
