/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Translation1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Penalities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Control = c.String(),
                        Reason = c.String(),
                        StartNumber = c.String(),
                        Value = c.Time(nullable: false, precision: 7),
                        StageNumber = c.Int(nullable: false),
                        EventTag_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.EventTag_Id)
                .Index(t => t.EventTag_Id);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartNumber = c.String(),
                        StageNumber = c.Int(nullable: false),
                        Value = c.Time(nullable: false, precision: 7),
                        Tag_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .Index(t => t.Tag_Id);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(),
                        Title = c.String(),
                        Sort = c.Int(nullable: false),
                        IsFinal = c.Boolean(nullable: false),
                        Tag_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .Index(t => t.Tag_Id);
            
            CreateTable(
                "dbo.Stages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        FinishTime = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Name = c.String(),
                        Slug = c.String(),
                        Map_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.Map_Id)
                .Index(t => t.Map_Id);
            
            CreateTable(
                "dbo.StageResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StageTime = c.Time(nullable: false, precision: 7),
                        StagePenality = c.Time(nullable: false, precision: 7),
                        StageTotal = c.Time(nullable: false, precision: 7),
                        IsRetired = c.Boolean(nullable: false),
                        Reason = c.String(),
                        OveralTime = c.Time(nullable: false, precision: 7),
                        OveralPenality = c.Time(nullable: false, precision: 7),
                        OveralTotal = c.Time(nullable: false, precision: 7),
                        PreviousStageTotal = c.Time(nullable: false, precision: 7),
                        Order_Id = c.Int(),
                        Stage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.Stages", t => t.Stage_Id)
                .Index(t => t.Order_Id)
                .Index(t => t.Stage_Id);
            
            CreateTable(
                "dbo.TargetTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartNumber = c.String(),
                        TargetSlug = c.String(),
                        Time = c.DateTime(),
                        Sequence = c.Int(),
                        EventTag_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.EventTag_Id)
                .Index(t => t.EventTag_Id);
            
            CreateTable(
                "dbo.StageSections",
                c => new
                    {
                        Stage_Id = c.Int(nullable: false),
                        Section_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Stage_Id, t.Section_Id })
                .ForeignKey("dbo.Stages", t => t.Stage_Id, cascadeDelete: true)
                .ForeignKey("dbo.Sections", t => t.Section_Id, cascadeDelete: true)
                .Index(t => t.Stage_Id)
                .Index(t => t.Section_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TargetTimes", "EventTag_Id", "dbo.Tags");
            DropForeignKey("dbo.Sections", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.StageSections", "Section_Id", "dbo.Sections");
            DropForeignKey("dbo.StageSections", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.StageResults", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.StageResults", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Stages", "Map_Id", "dbo.Files");
            DropForeignKey("dbo.Results", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Penalities", "EventTag_Id", "dbo.Tags");
            DropIndex("dbo.TargetTimes", new[] { "EventTag_Id" });
            DropIndex("dbo.Sections", new[] { "Tag_Id" });
            DropIndex("dbo.StageSections", new[] { "Section_Id" });
            DropIndex("dbo.StageSections", new[] { "Stage_Id" });
            DropIndex("dbo.StageResults", new[] { "Stage_Id" });
            DropIndex("dbo.StageResults", new[] { "Order_Id" });
            DropIndex("dbo.Stages", new[] { "Map_Id" });
            DropIndex("dbo.Results", new[] { "Tag_Id" });
            DropIndex("dbo.Penalities", new[] { "EventTag_Id" });
            DropTable("dbo.StageSections");
            DropTable("dbo.TargetTimes");
            DropTable("dbo.StageResults");
            DropTable("dbo.Stages");
            DropTable("dbo.Sections");
            DropTable("dbo.Results");
            DropTable("dbo.Penalities");
        }
    }
}
