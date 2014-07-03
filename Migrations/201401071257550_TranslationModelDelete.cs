/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TranslationModelDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stages", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Stages", "Map_Id", "dbo.Files");
            DropForeignKey("dbo.StageResults", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.StageResults", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.StageSections", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.StageSections", "Section_Id", "dbo.Sections");
            DropForeignKey("dbo.Sections", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Sections", "SubEvent_Id", "dbo.SubEvents");
            DropForeignKey("dbo.Penalities", "EventTag_Id", "dbo.Tags");
            DropForeignKey("dbo.Results", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TargetTimes", "EventTag_Id", "dbo.Tags");
            DropIndex("dbo.Stages", new[] { "Event_Id" });
            DropIndex("dbo.Stages", new[] { "Map_Id" });
            DropIndex("dbo.StageResults", new[] { "Order_Id" });
            DropIndex("dbo.StageResults", new[] { "Stage_Id" });
            DropIndex("dbo.StageSections", new[] { "Stage_Id" });
            DropIndex("dbo.StageSections", new[] { "Section_Id" });
            DropIndex("dbo.Sections", new[] { "Tag_Id" });
            DropIndex("dbo.Sections", new[] { "SubEvent_Id" });
            DropIndex("dbo.Penalities", new[] { "EventTag_Id" });
            DropIndex("dbo.Results", new[] { "Tag_Id" });
            DropIndex("dbo.TargetTimes", new[] { "EventTag_Id" });
            DropTable("dbo.Sections");
            DropTable("dbo.Stages");
            DropTable("dbo.StageResults");
            DropTable("dbo.Penalities");
            DropTable("dbo.Results");
            DropTable("dbo.TargetTimes");
            DropTable("dbo.StageSections");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StageSections",
                c => new
                    {
                        Stage_Id = c.Int(nullable: false),
                        Section_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Stage_Id, t.Section_Id });
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                        Event_Id = c.Int(),
                        Map_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        SubEvent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.TargetTimes", "EventTag_Id");
            CreateIndex("dbo.Results", "Tag_Id");
            CreateIndex("dbo.Penalities", "EventTag_Id");
            CreateIndex("dbo.Sections", "SubEvent_Id");
            CreateIndex("dbo.Sections", "Tag_Id");
            CreateIndex("dbo.StageSections", "Section_Id");
            CreateIndex("dbo.StageSections", "Stage_Id");
            CreateIndex("dbo.StageResults", "Stage_Id");
            CreateIndex("dbo.StageResults", "Order_Id");
            CreateIndex("dbo.Stages", "Map_Id");
            CreateIndex("dbo.Stages", "Event_Id");
            AddForeignKey("dbo.TargetTimes", "EventTag_Id", "dbo.Tags", "Id");
            AddForeignKey("dbo.Results", "Tag_Id", "dbo.Tags", "Id");
            AddForeignKey("dbo.Penalities", "EventTag_Id", "dbo.Tags", "Id");
            AddForeignKey("dbo.Sections", "SubEvent_Id", "dbo.SubEvents", "Id");
            AddForeignKey("dbo.Sections", "Tag_Id", "dbo.Tags", "Id");
            AddForeignKey("dbo.StageSections", "Section_Id", "dbo.Sections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StageSections", "Stage_Id", "dbo.Stages", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StageResults", "Stage_Id", "dbo.Stages", "Id");
            AddForeignKey("dbo.StageResults", "Order_Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.Stages", "Map_Id", "dbo.Files", "Id");
            AddForeignKey("dbo.Stages", "Event_Id", "dbo.Events", "Id");
        }
    }
}
