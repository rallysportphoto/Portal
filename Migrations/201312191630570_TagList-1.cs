/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagList1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "Event_Id", c => c.Int());
            CreateIndex("dbo.Tags", "Event_Id");
            AddForeignKey("dbo.Tags", "Event_Id", "dbo.Events", "Id");

            Sql("INSERT INTO [dbo].[Tags] ([Id],[Name],[Year],[Discriminator]) VALUES('Сезон 2014','Сезон 2014',2014,'Season')");
            Sql("INSERT INTO [dbo].[Tags] ([Id],[Name],[Year],[Discriminator]) VALUES('Сезон 2013','Сезон 2013',2013,'Season')");
            Sql("INSERT INTO [dbo].[Tags] ([Id],[Name],[Year],[Discriminator]) VALUES('Сезон 2012','Сезон 2012',2012,'Season')");
            Sql("INSERT INTO [dbo].[Tags] ([Id],[Name],[Year],[Discriminator]) VALUES('Сезон 2011','Сезон 2011',2011,'Season')");
            Sql("INSERT INTO [dbo].[Tags] ([Id],[Name],[Year],[Discriminator]) VALUES('Сезон 2010','Сезон 2010',2010,'Season')");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "Event_Id", "dbo.Events");
            DropIndex("dbo.Tags", new[] { "Event_Id" });
            DropColumn("dbo.Tags", "Event_Id");
        }
    }
}
