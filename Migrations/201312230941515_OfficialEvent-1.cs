/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficialEvent1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "Logo_Id", "dbo.Files");
            DropIndex("dbo.Events", new[] { "Logo_Id" });
            AddColumn("dbo.Events", "IsFeatured", c => c.Boolean());
            AddColumn("dbo.Events", "Intro_Id", c => c.Int());
            CreateIndex("dbo.Events", "Intro_Id");
            AddForeignKey("dbo.Events", "Intro_Id", "dbo.Files", "Id");
            DropColumn("dbo.Events", "Logo_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "Logo_Id", c => c.Int());
            DropForeignKey("dbo.Events", "Intro_Id", "dbo.Files");
            DropIndex("dbo.Events", new[] { "Intro_Id" });
            DropColumn("dbo.Events", "Intro_Id");
            DropColumn("dbo.Events", "IsFeatured");
            CreateIndex("dbo.Events", "Logo_Id");
            AddForeignKey("dbo.Events", "Logo_Id", "dbo.Files", "Id");
        }
    }
}
