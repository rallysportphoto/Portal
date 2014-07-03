/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondaryId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubEvents", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SubEvents", "SecondaryId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubEvents", "SecondaryId", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.SubEvents", "Id", c => c.Int(nullable: false));
        }
    }
}
