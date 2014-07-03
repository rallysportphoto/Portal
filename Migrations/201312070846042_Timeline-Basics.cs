/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimelineBasics : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Season = c.Int(nullable: false),
                        Title = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        Url = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Series",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Season = c.Int(nullable: false),
                        Title = c.String(),
                        ShortName = c.String(),
                        Discipline_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disciplines", t => t.Discipline_Id)
                .Index(t => t.Discipline_Id);
            
            CreateTable(
                "dbo.Disciplines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SeriesEvents",
                c => new
                    {
                        Series_Id = c.Int(nullable: false),
                        Event_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Series_Id, t.Event_Id })
                .ForeignKey("dbo.Series", t => t.Series_Id, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Event_Id, cascadeDelete: true)
                .Index(t => t.Series_Id)
                .Index(t => t.Event_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SeriesEvents", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.SeriesEvents", "Series_Id", "dbo.Series");
            DropForeignKey("dbo.Series", "Discipline_Id", "dbo.Disciplines");
            DropIndex("dbo.SeriesEvents", new[] { "Event_Id" });
            DropIndex("dbo.SeriesEvents", new[] { "Series_Id" });
            DropIndex("dbo.Series", new[] { "Discipline_Id" });
            DropTable("dbo.SeriesEvents");
            DropTable("dbo.Disciplines");
            DropTable("dbo.Series");
            DropTable("dbo.Events");
        }
    }
}
