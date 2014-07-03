/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeriesTags2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tags", "Season1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "Season1", c => c.Int());
        }
    }
}
