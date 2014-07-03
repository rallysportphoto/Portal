/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accreditation3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SeriesEvents", newName: "EventSeries");
            RenameTable(name: "dbo.TagFiles", newName: "FileTags");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.FileTags", newName: "TagFiles");
            RenameTable(name: "dbo.EventSeries", newName: "SeriesEvents");
        }
    }
}
