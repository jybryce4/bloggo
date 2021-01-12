-- Fixing my user profile creation
-- DECLARE @JBUsername NVARCHAR(30);
-- DECLARE @JBFirstName NVARCHAR(100);
-- DECLARE @JBLastName NVARCHAR(100);

-- SET @JBUsername = (
--     SELECT UserName FROM 
--     [dbo].[User] 
--     WHERE UserName = 'jybryce'
-- );

-- SET @JBFirstName = (
--     SELECT FirstName FROM 
--     [dbo].[User] 
--     WHERE UserName = 'jybryce'
-- );

-- SET @JBLastName = (
--     SELECT LastName FROM 
--     [dbo].[User] 
--     WHERE UserName = 'jybryce'
-- );

-- INSERT INTO [dbo].[Profile]
-- (UserName, FirstName, LastName, ProfileURL, ProfileImageURL, CoverImageURL, UserBio, Website, NumFollowers, Coins)
-- VALUES (
--     @JBUsername,
--     @JBFirstName,
--     @JBLastName,
--     'users/jybryce',
--     'img/jybryce.jpg',
--     'img/james-cover.jpg',
--     'Bloggo Developer',
--     'github.com/jybryce4',
--     0,
--     0
-- );



