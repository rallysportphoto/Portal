/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdersPrint3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubEvents", "TimingSystemId", c => c.Int());
            DropColumn("dbo.Events", "TimingSystemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "TimingSystemId", c => c.Int());
            DropColumn("dbo.SubEvents", "TimingSystemId");
        }
    }
}
