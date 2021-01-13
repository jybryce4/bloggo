INSERT INTO [dbo].[Post] ([Title], [Subtitle], [Content], [DatePosted], [Reblogs], [Upvotes])
VALUES (
    'Test Post',
    'A test post',
    'This is a test post inserted manually with SQL.',
    '2021-01-13',
    0,
    0
);

DECLARE @postid int;
SET @postid = (
    SELECT PostID
    FROM dbo.Post
    WHERE PostID = 1
);


-- have to insert into UserPost
INSERT INTO [dbo].[UserPost] ([PostID], [UserName])
VALUES (@postid, 'jybryce');