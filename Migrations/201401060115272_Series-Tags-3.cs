/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
namespace Portal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeriesTags3 : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[Tags] ( Id, Name, Discriminator, Series_Id)
SELECT TOP 1000 
	  [Series].[Title] + ' (' + [Disciplines].Title + ' '+ CAST([Season] as nvarchar(max)) + ')' as Id,
	  [Series].[Title] + ' (' + [Disciplines].Title + ' '+ CAST([Season] as nvarchar(max)) + ')' as Name,
	  'SeriesTag'
	  ,[Series].[Id] as Series_Id
  FROM [dbo].[Series]
  JOIN [dbo].[Disciplines] ON [dbo].[Disciplines].Id=[dbo].[Series].[Discipline_Id]
  WHERE [Series].Season=2014");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM [dbo].[Tags] WHERE Discriminator='SeriesTag'");
        }
    }
}
