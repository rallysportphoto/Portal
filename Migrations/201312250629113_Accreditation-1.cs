/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accreditation1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "AccreditationsEnabled", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "AccreditationsEnabled");
        }
    }
}
