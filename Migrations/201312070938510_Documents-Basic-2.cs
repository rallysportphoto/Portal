/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentsBasic2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "Season", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "Season");
        }
    }
}
