-- Соревнования
INSERT INTO [Portal.Models.SportDataContext].[dbo].[Events]
           ([Season]
           ,[Title]
           ,[StartDate]
           ,[FinishDate]           
           ,[Discriminator]
           ,[Place])
 SELECT 
2014 as Season,
Title,
Start,
Finish,
'Event' as Discriminator,
Place
FROM 
[RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].Timeline
WHERE SeasonId='DFE1F9FD-6734-4EBA-9041-CD9281523416' AND IsDeleted=0
GO

INSERT INTO [Portal.Models.SportDataContext].[dbo].[Events]
           ([Season]
           ,[Title]
           ,[StartDate]
           ,[FinishDate]           
           ,[Discriminator]
           ,[Place])
 SELECT 
2013 as Season,
Title,
Start,
Finish,
'Event' as Discriminator,
Place
FROM 
[RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].Timeline
WHERE SeasonId='9E0ED72F-18F9-4D37-85D3-97160B37D7B1' AND IsDeleted=0
GO

INSERT INTO [Portal.Models.SportDataContext].[dbo].[Events]
           ([Season]
           ,[Title]
           ,[StartDate]
           ,[FinishDate]           
           ,[Discriminator]
           ,[Place])
 SELECT 
2012 as Season,
Title,
Start,
Finish,
'Event' as Discriminator,
Place
FROM 
[RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].Timeline
WHERE SeasonId='3C3FA62E-2D84-4DA5-9DB4-0B36CE58E323' AND IsDeleted=0
GO

INSERT INTO [Portal.Models.SportDataContext].[dbo].[Events]
           ([Season]
           ,[Title]
           ,[StartDate]
           ,[FinishDate]           
           ,[Discriminator]
           ,[Place])
 SELECT 
2011 as Season,
Title,
Start,
Finish,
'Event' as Discriminator,
Place
FROM 
[RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].Timeline
WHERE SeasonId='623F057F-2A4B-410E-B641-D456F2111F7B' AND IsDeleted=0
GO

INSERT INTO [Portal.Models.SportDataContext].[dbo].[Events]
           ([Season]
           ,[Title]
           ,[StartDate]
           ,[FinishDate]           
           ,[Discriminator]
           ,[Place])
 SELECT 
2010 as Season,
Title,
Start,
Finish,
'Event' as Discriminator,
Place
FROM 
[RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].Timeline
WHERE SeasonId='5F651EA7-0C7A-443A-B482-8B51FF5CF842' AND IsDeleted=0
GO

--Список групп по сезонам 2010-2014
USE [Portal.Models.SportDataContext]
GO
/****** Object:  Table [dbo].[Disciplines]    Script Date: 12/10/2013 10:52:00 ******/
SET IDENTITY_INSERT [dbo].[Disciplines] ON
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (1, N'Картинг')
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (2, N'Трофи-рейд')
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (3, N'Автомногоборье')
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (4, N'Ледовые гонки')
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (5, N'Кросс')
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (6, N'Любительские ралли (Р3К)')
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (7, N'Ралли-Рейд')
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (8, N'Кольцо')
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (9, N'Трек')
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (10, N'Ралли-спринт')
INSERT [dbo].[Disciplines] ([Id], [Title]) VALUES (11, N'Ралли')
SET IDENTITY_INSERT [dbo].[Disciplines] OFF
/****** Object:  Table [dbo].[Series]    Script Date: 12/10/2013 10:52:00 ******/
SET IDENTITY_INSERT [dbo].[Series] ON
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (1, 2014, N'Традиционное', N'Т', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (2, 2014, N'Первернство России', N'ПР', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (3, 2014, N'Традиционное', N'Т', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (4, 2014, N'Чемпионат и Первенство Санкт-Петербурга', N'СПб', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (5, 2014, N'Традиционное', N'Т', 1)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (6, 2014, N'Кубок России', N'КР', 4)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (7, 2014, N'Кубок и Первенство Санкт-Петербурга', N'СПб', 6)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (8, 2014, N'Кубок России', N'КР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (9, 2014, N'Кубок СЗФО (клуб)', N'ККР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (10, 2014, N'Чемпионат России', N'ЧР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (11, 2014, N'Чемпионат СЗФО', N'ЧСЗФО', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (12, 2014, N'Кубок Мира', N'КМ', 7)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (13, 2014, N'Кубок России', N'КР', 7)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (14, 2014, N'Чемпионат России', N'ЧР', 7)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (15, 2014, N'Кубок Ленинградской области', N'КЛО', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (16, 2014, N'Кубок Лужского района', N'КЛуж', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (17, 2014, N'Кубок РО ДОСААФ (зима)', N'КДосааф', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (18, 2014, N'Луга Традиционное', N'ТЛ', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (19, 2014, N'Традиционное', N'Т', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (20, 2014, N'Кубок РО ДОСААФ (зима)', N'КДосааф', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (21, 2014, N'Традиционное', N'Т', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (22, 2014, N'Международное', N'МН', 2)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (23, 2014, N'Чемпионат России', N'ЧР', 2)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (24, 2013, N'Первенство России', N'ПР', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (25, 2013, N'Чемпионат и Первенство Санкт-Петербурга', N'СПб', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (26, 2013, N'традиционное', N'Т', 1)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (27, 2013, N'Кубок России', N'КР', 4)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (28, 2013, N'Кубок и Первенство Санкт-Петербурга', N'СПб', 6)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (29, 2013, N'традиционное', N'Т', 6)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (30, 2013, N'Кубок Ленинградской области по клубному ралли', N'КЛО(клуб)', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (31, 2013, N'Кубок России', N'КР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (32, 2013, N'Кубок СЗФО (клуб)', N'ККР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (33, 2013, N'Чемпионат России', N'ЧР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (34, 2013, N'Чемпионат СЗФО', N'ЧСЗФО', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (35, 2013, N'традиционное', N'Т', 7)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (36, 2013, N'Чемпионат России', N'ЧР', 7)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (37, 2013, N'Кубок Ленинградской области', N'КЛО', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (38, 2013, N'Кубок Лужского района', N'КЛуж', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (39, 2013, N'Кубок РО ДОСААФ (зима)', N'КДосааф', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (40, 2013, N'Кубок РО ДОСААФ (лето)', N'КДосааф (лето)', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (41, 2013, N'традиционное', N'Т', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (42, 2013, N'Кубок РО ДОСААФ (зима)', N'КДосааф', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (44, 2013, N'Кубок РО ДОСААФ (лето)', N'КДосааф', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (45, 2013, N'традиционное', N'Т', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (46, 2013, N'Международное', N'МН', 2)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (47, 2013, N'Чемпионат России', N'ЧР', 2)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (48, 2012, N'всероссийское', N'ВС', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (49, 2012, N'Кубок России', N'КР', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (50, 2012, N'Первенство России', N'ПР', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (51, 2012, N'традиционное', N'Т', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (52, 2012, N'Чемпионат России', N'ЧР', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (53, 2012, N'Первенство России', N'ПР', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (54, 2012, N'Чемпионат и Первенство Санкт-Петербурга', N'СПб', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (55, 2012, N'Чемпионат России', N'ЧР', 8)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (56, 2012, N'традиционное', N'Т', 1)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (57, 2012, N'Кубок России', N'КР', 4)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (58, 2012, N'Кубок и Первенство Санкт-Петербурга', N'СПб', 6)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (59, 2012, N'Традиционное', N'Т', 6)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (60, 2012, N'Кубок Ленинградской области по клубному ралли', N'КЛО(клуб)', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (61, 2012, N'Кубок России', N'КР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (62, 2012, N'Кубок СЗФО (клуб)', N'ККР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (63, 2012, N'Чемпионат Ленинградской области', N'ЧЛО', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (64, 2012, N'Чемпионат России', N'ЧР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (65, 2012, N'Чемпионат СЗФО', N'ЧСЗФО', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (66, 2012, N'Кубок Ленинградской области', N'КЛО', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (67, 2012, N'Кубок Лужского района', N'КЛуж', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (68, 2012, N'Кубок РО ДОСААФ (зима)', N'КДосааф', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (69, 2012, N'Кубок РО ДОСААФ (лето)', N'КДосааф(Л)', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (70, 2012, N'Луга_традиционное', N'Луга_Т', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (71, 2012, N'традиционное', N'Т', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (72, 2012, N'Кубок РО ДОСААФ (зима)', N'КДосааф', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (73, 2012, N'Кубок РО ДОСААФ (лето)', N'КДосааф(Л)', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (75, 2012, N'традиционное', N'Т', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (76, 2012, N'международное', N'МН', 2)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (77, 2011, N'всероссийское', N'вс', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (78, 2011, N'Кубок России', N'КР', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (79, 2011, N'Первенство России', N'ПР', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (80, 2011, N'традиционное', N'Т', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (81, 2011, N'Чемпионат России', N'ЧР', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (82, 2011, N'Открытые областные соревнования Псковская область', N'ОткПск', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (83, 2011, N'Первенство России', N'ПР', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (84, 2011, N'традиционное', N'Т', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (85, 2011, N'Чемпионат и Первенство Санкт-Петербурга', N'СПб', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (86, 2011, N'Чемпионат Псковской области', N'ЧПск', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (87, 2011, N'Открытые областные соревнования Псковская область', N'ОткПск', 1)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (88, 2011, N'традиционное', N'Т', 1)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (89, 2011, N'Кубок России', N'КР', 4)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (90, 2011, N'Кубок Санкт-Петербурга', N'КСПб', 4)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (91, 2011, N'Кубок Северо-Запада', N'КСЗ', 4)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (92, 2011, N'традиционное', N'Т', 4)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (93, 2011, N'Кубок и Первенство Санкт-Петербурга', N'СПб', 6)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (94, 2011, N'традиционное', N'Т', 6)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (95, 2011, N'Кубок Ленинградской области по клубному ралли', N'КЛО', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (96, 2011, N'Кубок России', N'КР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (97, 2011, N'Кубок СЗФО (клуб)', N'ККР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (98, 2011, N'Чемпионат Ленинградской области', N'ЧЛО', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (99, 2011, N'Чемпионат России', N'ЧР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (100, 2011, N'Чемпионат СЗФО', N'ЧСЗФО', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (101, 2011, N'Кубок Мира', N'КМ', 7)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (102, 2011, N'Кубок России', N'КР', 7)
GO
print 'Processed 100 total records'
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (103, 2011, N'традиционное', N'Т', 7)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (104, 2011, N'Чемпионат России', N'ЧР', 7)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (105, 2011, N'Кубок Лужского района', N'КЛуж', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (106, 2011, N'Кубок РО ДОСААФ (зима)', N'КДосааф', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (107, 2011, N'Кубок РО ДОСААФ (лето)', N'КДосааф(Л)', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (108, 2011, N'Луга_традиционное', N'Луга_Т', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (109, 2011, N'традиционное', N'Т', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (110, 2011, N'Кубок РО ДОСААФ (зима)', N'КДосааф', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (111, 2011, N'Кубок РО ДОСААФ (лето)', N'КДосааф(Л)', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (113, 2011, N'Открытый кубок Санкт-Петербурга', N'СПб', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (114, 2011, N'традиционное', N'Т', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (116, 2011, N'международное', N'МН', 2)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (117, 2011, N'Чемпионат России', N'ЧР', 2)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (118, 2010, N'всероссийское', N'ВС', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (119, 2010, N'Первенство России', N'ПР', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (120, 2010, N'Чемпионат России', N'ЧР', 5)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (121, 2010, N'Открытые областные соревнования Псковская область', N'ОткПск', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (122, 2010, N'Чемпионат и Первенство Санкт-Петербурга', N'СПб', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (123, 2010, N'Чемпионат Псковской области', N'ЧПск', 3)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (125, 2010, N'Открытые областные соревнования Псковская область', N'ОткПск', 1)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (126, 2010, N'Кубок России', N'КР', 4)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (127, 2010, N'Кубок Северо-Запада', N'КСЗ', 4)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (128, 2010, N'Кубок и Первенство Санкт-Петербурга', N'СПб', 6)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (130, 2010, N'Кубок России', N'КР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (131, 2010, N'Кубок СЗФО (клуб)', N'ККР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (132, 2010, N'Чемпионат Ленинградской области', N'ЧЛО', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (133, 2010, N'Чемпионат России', N'ЧР', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (134, 2010, N'Чемпионат СЗФО', N'ЧСЗФО', 11)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (135, 2010, N'Кубок Мира', N'КМ', 7)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (136, 2010, N'Чемпионат России', N'ЧР', 7)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (137, 2010, N'традиционное', N'Т', 10)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (138, 2010, N'Открытый кубок Санкт-Петербурга', N'СПб', 9)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (139, 2010, N'международное', N'МН', 2)
INSERT [dbo].[Series] ([Id], [Season], [Title], [ShortName], [Discipline_Id]) VALUES (140, 2010, N'Чемпионат России', N'ЧР', 2)
SET IDENTITY_INSERT [dbo].[Series] OFF

--- Выбор групп по сезонам
SELECT DISTINCT [EventTypes].Name,[EventCategory].Name, [EventCategory].ShortName  FROM 
[RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].Timeline
JOIN [RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].TimelineEventCategories ON [Timeline].Id = TimelineEventCategories.[TimelineId]
JOIN [RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].[EventCategory] ON TimelineEventCategories.[EventCategoryId] = EventCategory.Id
JOIN [RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].[EventTypes] ON [EventCategory].[EventTypeId] = [EventTypes].Id
WHERE SeasonId='DFE1F9FD-6734-4EBA-9041-CD9281523416'


---NEWS
INSERT INTO [Portal.Models.SportDataContext].[dbo].[News]
           ([Slug]
           ,[Title]
           ,[Body]
           ,[Date]
           ,[Season]
           ,[Type])
SELECT 
Id as Slug, 
Title,
Description as Body,
CreatedAt as [Date],
2014 as Season,
0 as Type 
FROM [RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].[News]
WHERE CreatedAt >= '2013-11-20'


INSERT INTO [Portal.Models.SportDataContext].[dbo].[News]
           ([Slug]
           ,[Title]
           ,[Body]
           ,[Date]
           ,[Season]
           ,[Type])
SELECT 
Id as Slug, 
Title,
Description as Body,
CreatedAt as [Date],
2013 as Season,
0 as Type 
FROM [RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].[News]
WHERE CreatedAt < '2013-11-20' AND CreatedAt > '2012-11-20'
ORDER BY Date

INSERT INTO [Portal.Models.SportDataContext].[dbo].[News]
           ([Slug]
           ,[Title]
           ,[Body]
           ,[Date]
           ,[Season]
           ,[Type])
SELECT 
Id as Slug, 
Title,
Description as Body,
CreatedAt as [Date],
2012 as Season,
0 as Type 
FROM [RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].[News]
WHERE CreatedAt < '2012-11-20' AND CreatedAt > '2011-11-20'
ORDER BY Date


INSERT INTO [Portal.Models.SportDataContext].[dbo].[News]
           ([Slug]
           ,[Title]
           ,[Body]
           ,[Date]
           ,[Season]
           ,[Type])
SELECT 
Id as Slug, 
Title,
Description as Body,
CreatedAt as [Date],
2011 as Season,
0 as Type 
FROM [RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].[News]
WHERE CreatedAt < '2011-11-20' AND CreatedAt > '2010-11-15'
ORDER BY Date

INSERT INTO [Portal.Models.SportDataContext].[dbo].[News]
           ([Slug]
           ,[Title]
           ,[Body]
           ,[Date]
           ,[Season]
           ,[Type])
SELECT 
Id as Slug, 
Title,
Description as Body,
CreatedAt as [Date],
2010 as Season,
0 as Type 
FROM [RSP-DATABASE.CLOUDAPP.NET].[afspb].[dbo].[News]
WHERE CreatedAt < '2010-11-15' AND CreatedAt > '2009-10-20'
ORDER BY Date