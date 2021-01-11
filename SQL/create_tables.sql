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
    Email NVARCHAR(60) NOT NULL,
    CONSTRAINT PK_Users PRIMARY KEY (UserID, Username)
);

-- Profiles table stores data for generating profile pages
CREATE TABLE [dbo].[Profiles]
(
    ProfileID BIGINT NOT NULL IDENTITY,
    UserName NVARCHAR(30) NOT NULL UNIQUE,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    ProfileURL NVARCHAR(100) NOT NULL UNIQUE,
    ProfileImageURL NVARCHAR(200),
    CoverImageURL NVARCHAR(200),
    UserBio NVARCHAR(4000),
    Website NVARCHAR(100),
    NumFollowers BIGINT,
    Coins BIGINT,
    CONSTRAINT FK_UserName_Profiles FOREIGN KEY (UserName) REFERENCES dbo.Users (UserName)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    CONSTRAINT PK_Profiles PRIMARY KEY (ProfileID)
);

-- -- LoggedInUsers stores users who are currently logged into the site
-- CREATE TABLE LoggedInUsers
-- (
--     -- TO DO
-- );




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



