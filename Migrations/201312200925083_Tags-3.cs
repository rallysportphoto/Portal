/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tags3 : DbMigration
    {
        public override void Up()
        {
            RenameTable("TagNews", "NewsTags");
        }
        
        public override void Down()
        {
            RenameTable("NewsTags", "TagNews");
        }
    }
}
