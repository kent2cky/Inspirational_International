---- README README README README README README README     -----
---- This is a script used to create and manage the       -----
---- Comments table. The script in itself is self----
---- explaining. However it is well commented so that who- ----
---- ever reads it will understand what is going on here. -----
---- The scripts are commented out by default. Please     -----
---- uncomment just the part you want to work with. You can ---
---- execute that part individually and others will not   -----
---- interfere.  Remember, to execute this script press -------
----- ctrl + shift e (on .net core in vscode).                                      -----
-----
---- Goodluck!                                            -----
---- README README README README README README README     -----

---- I'm keeping some of the queries here as reminders
---- in the future.

-- SELECT name, database_id, create_date
-- FROM sys.databases ;
-- GO

-- DROP DATABASE Transactions;
-- Go

-- CREATE DATABASE InspInt_Database;
-- GO

SELECT *
FROM InspInt_Database.INFORMATION_SCHEMA.TABLES;
GO
-- SELECT *
-- FROM AspNetUserClaims;
-- GO
-- SELECT *
-- FROM INFORMATION_SCHEMA.TABLES

-- GO

-- Create a new table called 'Comments' in schema 'dbo'
-- Drop the table if it already exists

-- IF OBJECT_ID('dbo.Comments', 'U') IS NOT NULL
-- DROP TABLE dbo.Comments
-- GO

-- Create the table in the specified schema

-- CREATE TABLE dbo.Comments
-- (
--     Comment_ID INT NOT NULL IDENTITY,
--     Date_Time [DATETIME] NOT NULL,
--     --- Date and time when the comment was posted.
--     Comment_Body [NVARCHAR] (MAX) NOT NULL,
--     _Name [NVARCHAR] (MAX) NOT NULL,
--     --- Name of the commentor
--     Article_ID INT NOT NULL,

--     PRIMARY KEY (Comment_ID),
--     CONSTRAINT FK_ArticleID
--     FOREIGN KEY (Article_ID)
--     REFERENCES dbo.Articles(Article_ID)
--     --- Foreign key referencing the articles table.
--     ON DELETE CASCADE
--     ON UPDATE CASCADE
--     --- changes in articles table affects this table.
-- );
-- GO


-- Insert rows into table 'Comments'

-- INSERT INTO Comments
--     ( -- columns to insert data into
--     [Date_Time], [Comment_Body],
--     [_Name], [Article_ID]
--     )
-- VALUES
--     ( -- first row: values for the columns in the list above
--         CONVERT([DATETIME], '19-02-19 9:55:56 PM', 5), 'Great Article, Coach. Keep it coming!',
--         'Ugochukwu Maduka', 1
-- ),
--     ( -- second row: values for the columns in the list above
--         CONVERT([DATETIME], '12-02-19 8:15:56 PM', 5), 'Nice one. Quit insightful, thanks a lot.',
--         'Kenny Maddy', 3
-- ),
--     ( -- third row: values for the columns in the list above
--         CONVERT([DATETIME], '11-02-19 7:23:26 PM', 5), 'Great Job, Coach. Can I send you an Email?',
--         'Ciroma Adekunle Chukwuma', 3      
-- ),
--     ( -- fourth row: values for the columns in the list above
--         CONVERT([DATETIME], '13-02-19 6:51:26 PM', 5), 'Thanks a lot for your kind words.
--         I am glad you guys like the article.',
--         'Coach David', 4
-- )
-- -- add more rows here
-- GO

-- -- Create a stored procedure
-- -- to add an Comments to the Comments table


-- CREATE PROCEDURE spSubmitComment
--     (
--     @Date_Time [DATETIME],
--     @Comment_Body [NVARCHAR] (MAX),
--     @_Name [NVARCHAR] (MAX),
--     @Article_ID [INT]
-- -- Parameters
-- )
-- AS
-- BEGIN
--     INSERT INTO Comments
--         (

--         -- columns to insert data into

--         [Date_Time], [Comment_Body],
--         [_Name], [Article_ID]
--         )
--     VALUES
--         (@Date_Time, @Comment_Body, @_Name,
--             @Article_ID)
-- END

-- Create a stored procedure
-- to update an article on the
-- Comments table.

-- CREATE PROCEDURE spUpdateComment
--     (
--     @Comment_ID [INT],
--     @Date_Time [DATETIME],
--     @Comment_Body [NVARCHAR] (MAX),
--     @_Name [NVARCHAR] (MAX),
--     @Article_ID [INT]
-- )
-- AS
-- BEGIN
--     UPDATE Comments

--  -- columns to insert updates into

--     SET [Date_Time]=@Date_Time, [_Name]=@_Name,
--          [Comment_Body]=@Comment_Body,
--          [Article_ID]=@Article_ID
--     WHERE Comment_ID=@Comment_ID
-- END

-- Create a stored procedure
-- To delete a record
-- from the Comments table


-- CREATE PROCEDURE spDeleteComment
--     (
--     @Comment_ID [INT]
-- )
-- AS
-- BEGIN
--     DELETE FROM Comments WHERE Comment_ID=@Comment_ID
-- END


-- Create a stored procedure to
-- retrieve all Comments recorded on the Comments table

-- DROP PROCEDURE dbo.spGetAllComments;
-- GO

-- CREATE PROCEDURE spGetAllComments
-- AS
-- BEGIN
--     SELECT *
--     FROM dbo.Comments
-- END

-- Create a stored procedure to
-- retrieve a single Comments on the Comments table

-- CREATE PROCEDURE spGetSingleComment
--     (
--     @Comment_ID [INT]
-- )
-- AS
-- BEGIN
--     SELECT *
--     FROM Comments
--     WHERE @Comment_ID=Comment_ID
-- END


-- EXEC dbo.spDeleteComment 9;
-- GO

-- DROP PROCEDURE dbo.spSubmitComment;
-- GO

-- DECLARE @datefor DATETIME
-- DECLARE @name NVARCHAR(MAX)
-- DECLARE @Contact NVARCHAR(MAX)
-- DECLARE @Did_Attend NVARCHAR(MAX)

-- SET @datefor = '2019-05-10T10:29:34';
-- SET @name = 'Ugochukwu';
-- SET @Contact = 'No comments.';
-- SET @Did_Attend = 10;

-- -- EXEC dbo.spUpdateComment 4, @datefor, @Contact, @name, @Did_Attend
-- -- GO

-- EXEC dbo.spSubmitComment @datefor, @Contact ,@name, 8;
-- GO

-- DROP PROCEDURE dbo.spSubmitComments
-- GO

-- DELETE FROM Comments WHERE ID = 2;
-- GO

-- EXEC dbo.spGetSingleComment 8;
-- GO

EXEC dbo.spGetAllComments ;
GO

-- SELECT *
-- FROM Comments
-- GO

--- Some of the queries contained in this script are for reference purposes
--- in case I need them in the future.