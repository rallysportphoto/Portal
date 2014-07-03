/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bugfixing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Series", "Slug", c => c.String());
            AddColumn("dbo.Sections", "SubEvent_Id", c => c.Int());
            CreateIndex("dbo.Sections", "SubEvent_Id");
            AddForeignKey("dbo.Sections", "SubEvent_Id", "dbo.SubEvents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sections", "SubEvent_Id", "dbo.SubEvents");
            DropIndex("dbo.Sections", new[] { "SubEvent_Id" });
            DropColumn("dbo.Sections", "SubEvent_Id");
            DropColumn("dbo.Series", "Slug");
        }
    }
}
