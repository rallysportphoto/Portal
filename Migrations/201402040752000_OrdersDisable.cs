/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdersDisable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "OrdersDisabled", c => c.Boolean());
            Sql("UPDATE [dbo].[Events] SET OrdersDisabled = 0 WHERE [Discriminator] = 'OfficialEvent'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "OrdersDisabled");
        }
    }
}
