/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accreditation0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accreditations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Car = c.String(),
                        Tag_Id = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Tag_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.MediaInfoEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Media = c.String(),
                        Position = c.String(),
                        Organisation = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Web = c.String(),
                        Region = c.String(),
                        Frequency = c.String(),
                        Edition = c.String(),
                        Accreditation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accreditations", t => t.Accreditation_Id)
                .Index(t => t.Accreditation_Id);
            
            CreateTable(
                "dbo.Licenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Season = c.Int(nullable: false),
                        Number = c.String(),
                        IssuesOn = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Users", "LastName", c => c.String());
            AddColumn("dbo.Users", "FirstName", c => c.String());
            AddColumn("dbo.Users", "Location", c => c.String());
            AddColumn("dbo.Users", "Phone", c => c.String());
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "BirthDate", c => c.DateTime());
            AddColumn("dbo.Users", "Passport", c => c.String());
            AddColumn("dbo.Users", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accreditations", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Licenses", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Accreditations", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.MediaInfoEntries", "Accreditation_Id", "dbo.Accreditations");
            DropIndex("dbo.Accreditations", new[] { "User_Id" });
            DropIndex("dbo.Licenses", new[] { "User_Id" });
            DropIndex("dbo.Accreditations", new[] { "Tag_Id" });
            DropIndex("dbo.MediaInfoEntries", new[] { "Accreditation_Id" });
            DropColumn("dbo.Users", "Address");
            DropColumn("dbo.Users", "Passport");
            DropColumn("dbo.Users", "BirthDate");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "Phone");
            DropColumn("dbo.Users", "Location");
            DropColumn("dbo.Users", "FirstName");
            DropColumn("dbo.Users", "LastName");
            DropTable("dbo.Licenses");
            DropTable("dbo.MediaInfoEntries");
            DropTable("dbo.Accreditations");
        }
    }
}
