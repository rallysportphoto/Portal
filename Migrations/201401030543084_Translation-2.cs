/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Translation2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        JobName = c.String(maxLength: 64),
                        WorkerId = c.String(maxLength: 64),
                        Started = c.DateTime(nullable: false),
                        Completed = c.DateTime(),
                        ExceptionInfo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WorkItems");
        }
    }
}
