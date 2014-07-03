/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Roles2 : DbMigration
    {
        public override void Up()
        {
//            Sql(@"INSERT INTO [dbo].[AspNetRoles]
//           ([Id]
//           ,[Name])
//     VALUES
//           ('Accreditations','Accreditations'),
//			('Docs','Docs'),
//			('Events','Events'),
//			('News','News'),
//			('Users','Users'),
//			('Orders','Orders')");
        }
        
        public override void Down()
        {
        }
    }
}
