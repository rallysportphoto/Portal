/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializationFix : DbMigration
    {
        public override void Up()
        {
            this.Sql(@"INSERT INTO [dbo].[Users]
           ([Id]
           ,[UserName]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[Discriminator]
           ,[LastName]
           ,[FirstName]           
           ,[Email])
     VALUES
           ('0e2d0e9b-3f64-4390-b38a-a2d814d62f49',
           'fde',
           'AM0ZD1Omnj5PmGs9ByZOJbiZ+KBcQqVnmf1UYLWXeSUkeo7QBr8rH9w7qI7vfM1Ctg==',
		   'f4be215d-884d-45e5-b583-ace0da87b44a',
           'User',
		   'Fedorov',           
           'Dmitry',		   
		   'fde@mail.ru')");

            this.Sql(@"INSERT INTO [dbo].[AspNetRoles]
           ([Id]
           ,[Name])
     VALUES
           ('Events','Events'),
		   ('Orders','Orders'),
		   ('News','News'),
		   ('Media','Media'),
		   ('Docs','Docs'),
		   ('Accreditations','Accreditations')");

            Sql(@"INSERT INTO [dbo].[AspNetUserRoles]
           ([UserId]
           ,[RoleId])
     VALUES
           ('0e2d0e9b-3f64-4390-b38a-a2d814d62f49','Events'),
		   ('0e2d0e9b-3f64-4390-b38a-a2d814d62f49','Orders'),
		   ('0e2d0e9b-3f64-4390-b38a-a2d814d62f49','News'),
		   ('0e2d0e9b-3f64-4390-b38a-a2d814d62f49','Media'),
		   ('0e2d0e9b-3f64-4390-b38a-a2d814d62f49','Docs'),
		   ('0e2d0e9b-3f64-4390-b38a-a2d814d62f49','Accreditations')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM [dbo].[AspNetUserRoles]");
            Sql("DELETE FROM [dbo].[AspNetRoles]");
            this.Sql("DELETE FROM [dbo].[Users] WHERE Id = '0e2d0e9b-3f64-4390-b38a-a2d814d62f49'");
            
        }
    }
}
