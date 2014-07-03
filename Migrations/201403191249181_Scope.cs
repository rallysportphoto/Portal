/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Scope : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Scope", c => c.String());
            DropColumn("dbo.Tags", "SiteName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "SiteName", c => c.String());
            DropColumn("dbo.Events", "Scope");
        }
    }
}
