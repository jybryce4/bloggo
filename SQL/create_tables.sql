-- User Tables
--------------------------------------------------------------------------------------------

-- Users table stores data core to the user for registration/login and credential retrieval
CREATE TABLE [dbo].[Users] 
(
    UserName NVARCHAR(30) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(60) NOT NULL,
    CONSTRAINT PK_Users PRIMARY KEY (Username)
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


-- Site Object Tables
--------------------------------------------------------------------------------------------------

-- Bridge entity for Profiles and Posts
CREATE TABLE [dbo].[UserPosts]
(
    UserPostID BIGINT NOT NULL IDENTITY PRIMARY KEY,
    PostID BIGINT NOT NULL,
    UserName NVARCHAR(30) NOT NULL,
    CONSTRAINT FK_UserNameID_UserPosts FOREIGN KEY (UserName) REFERENCES dbo.Profiles (UserName)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

-- Definition for how Posts are stored on the server
CREATE TABLE [dbo].[Posts]
(
    PostID BIGINT NOT NULL IDENTITY PRIMARY KEY,
    UserPostID BIGINT NOT NULL UNIQUE,
    Title NVARCHAR(60),
    Subtitle NVARCHAR(60),
    Content NVARCHAR(MAX) NOT NULL,
    DatePosted DATE,
    CONSTRAINT FK_UserPostID_Posts FOREIGN KEY (UserPostID) REFERENCES dbo.UserPosts (UserPostID)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

-- Adding FK constraint
ALTER TABLE [dbo].[UserPosts]
ADD CONSTRAINT FK_PostID_UserPosts FOREIGN KEY (PostID) REFERENCES dbo.Posts (PostID)
ON DELETE CASCADE
ON UPDATE CASCADE;





