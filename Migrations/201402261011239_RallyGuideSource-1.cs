/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RallyGuideSource1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubEvents", "SecondaryId", c => c.Guid(nullable: false, identity: true));
            AddColumn("dbo.SubEvents", "Logo_Id", c => c.Int());
            AddColumn("dbo.Events", "EmergencyPhone", c => c.String());
            AlterColumn("dbo.SubEvents", "Id", c => c.Int(nullable: false));
            CreateIndex("dbo.SubEvents", "Logo_Id");
            AddForeignKey("dbo.SubEvents", "Logo_Id", "dbo.Files", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubEvents", "Logo_Id", "dbo.Files");
            DropIndex("dbo.SubEvents", new[] { "Logo_Id" });
            AlterColumn("dbo.SubEvents", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Events", "EmergencyPhone");
            DropColumn("dbo.SubEvents", "Logo_Id");
            DropColumn("dbo.SubEvents", "SecondaryId");
        }
    }
}
