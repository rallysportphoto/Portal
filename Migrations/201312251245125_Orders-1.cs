/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "EventTag_Id", "dbo.Tags");
            DropIndex("dbo.Groups", new[] { "EventTag_Id" });
            AddColumn("dbo.Groups", "SubEvent_Id", c => c.Int());
            CreateIndex("dbo.Groups", "SubEvent_Id");
            AddForeignKey("dbo.Groups", "SubEvent_Id", "dbo.SubEvents", "Id");
            DropColumn("dbo.Groups", "EventTag_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "EventTag_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Groups", "SubEvent_Id", "dbo.SubEvents");
            DropIndex("dbo.Groups", new[] { "SubEvent_Id" });
            DropColumn("dbo.Groups", "SubEvent_Id");
            CreateIndex("dbo.Groups", "EventTag_Id");
            AddForeignKey("dbo.Groups", "EventTag_Id", "dbo.Tags", "Id");
        }
    }
}
