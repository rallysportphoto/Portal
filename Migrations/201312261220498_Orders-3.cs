/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Engine", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "Engine");
        }
    }
}
