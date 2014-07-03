/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficialEvent2 : DbMigration
    {
        public override void Up()
        {            
            DropForeignKey("dbo.SubEvents", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Tags", "Event_Id", "dbo.Events","Id");
            DropIndex("dbo.Tags", new[] { "Event_Id" });
            DropIndex("dbo.SubEvents", new[] { "Tag_Id" });
            CreateIndex("dbo.Tags", "Event_Id");
            AddForeignKey("dbo.Tags", "Event_Id", "dbo.SubEvents", "Id");            
        }
        
        public override void Down()
        {         
            DropForeignKey("dbo.Tags", "Event_Id", "dbo.SubEvents");
            DropIndex("dbo.Tags", new[] { "Event_Id" });
            CreateIndex("dbo.SubEvents", "Tag_Id");
            AddForeignKey("dbo.SubEvents", "Tag_Id", "dbo.Tags", "Id");         
        }
    }
}
