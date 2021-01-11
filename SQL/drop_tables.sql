ALTER TABLE [dbo].[UserPost]
DROP CONSTRAINT FK_UserName_UserPosts;

DROP TABLE [dbo].[UserPost];

DROP TABLE [dbo].[Post];

ALTER TABLE [dbo].[Profile]
DROP CONSTRAINT FK_UserName_Profiles;

DROP TABLE [dbo].[Profile];

DROP TABLE [dbo].[User];
