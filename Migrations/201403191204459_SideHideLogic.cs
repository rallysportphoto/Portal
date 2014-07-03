/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SideHideLogic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "SiteName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "SiteName");
        }
    }
}