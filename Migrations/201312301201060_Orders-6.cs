/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CreatedBy_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "CreatedBy_Id");
            AddForeignKey("dbo.Orders", "CreatedBy_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "CreatedBy_Id", "dbo.Users");
            DropIndex("dbo.Orders", new[] { "CreatedBy_Id" });
            DropColumn("dbo.Orders", "CreatedBy_Id");
        }
    }
}
