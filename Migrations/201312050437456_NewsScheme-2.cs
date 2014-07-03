/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewsScheme2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tags", "Year", c => c.Int());
            AddColumn("dbo.Tags", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "Discriminator");
            DropColumn("dbo.Tags", "Year");
            DropColumn("dbo.News", "Date");
        }
    }
}
