/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Partners2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partners", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Partners", "Position");
        }
    }
}
