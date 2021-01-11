-- User Tables
--------------------------------------------------------------------------------------------

-- Users table stores data core to the user for registration/login and credential retrieval
CREATE TABLE [dbo].[Users] 
(
    UserID BIGINT NOT NULL IDENTITY,
    UserName NVARCHAR(30) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Birthday DATE NOT NULL, -- YYYY-MM-DD
    Email NVARCHAR(60) NOT NULL,
    CONSTRAINT PK_Users PRIMARY KEY (UserID, Username)
);

-- Profiles table stores data for generating profile pages
CREATE TABLE [dbo].[Profiles]
(
    ProfileID BIGINT NOT NULL IDENTITY,
    UserName NVARCHAR(30) NOT NULL UNIQUE,
    ProfileURL NVARCHAR(100) NOT NULL UNIQUE,
    ProfileImageURL NVARCHAR(200),
    CoverImageURL NVARCHAR(200),
    UserBio NVARCHAR(4000),
    Website NVARCHAR(100)
    CONSTRAINT FK_UserName_Profiles FOREIGN KEY (UserName) REFERENCES dbo.Users (UserName)
);




-- Site Object Tables
--------------------------------------------------------------------------------------------------

-- CREATE TABLE [dbo].[Posts]
-- (
--     PostID BIGINT NOT NULL IDENTITY PRIMARY KEY,
--     UserName NVARCHAR(30) NOT NULL,
--     Title NVARCHAR(60),
--     Content NVARCHAR(MAX) NOT NULL,
--     DatePosted DATE,
--     CONSTRAINT FK_UserName_Users FOREIGN KEY (UserName) REFERENCES dbo.Users (UserName)
--     ON DELETE CASCADE
--     ON UPDATE CASCADE
-- );



