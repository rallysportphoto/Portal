/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdersPrint1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "TimingSystemId", c => c.Int());
            AddColumn("dbo.Orders", "TimingSystemId", c => c.Int());

        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "TimingSystemId");
            DropColumn("dbo.Orders", "TimingSystemId");
        }
    }
}
