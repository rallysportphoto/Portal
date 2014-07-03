/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders4 : DbMigration
    {
        public override void Up()
        {
            RenameTable("GroupOrders", "OrderGroups");
        }
        
        public override void Down()
        {
            RenameTable("OrderGroups", "GroupOrders");
        }
    }
}
