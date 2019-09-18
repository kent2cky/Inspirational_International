---- README README README README README README README     -----
---- This is a script used to create and manage the       -----
---- RSVP table. The script in itself is self----
---- explaining. However it is well commented so that who- ----
---- ever reads it will understand what is going on here. -----
---- The scripts are commented out by default. Please     -----
---- uncomment just the part you want to work with. You can ---
---- execute that part individually and others will not   -----
---- interfere.                                           -----
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

-- SELECT *
-- FROM INFORMATION_SCHEMA.TABLES
-- GO

-- Create a new table called 'RSVP' in schema 'dbo'
-- Drop the table if it already exists

-- IF OBJECT_ID('dbo.RSVP', 'U') IS NOT NULL
-- DROP TABLE dbo.RSVP
-- GO

-- Create the table in the specified schema

-- CREATE TABLE dbo.RSVP
-- (
--     ID INT NOT NULL IDENTITY PRIMARY KEY,
--     -- primary key column
--     Date_For [DATETIME] NOT NULL,
--     --- Date when the next class is going to hold.
--     Contact [NVARCHAR] (MAX) NULL,
--     _Name [NVARCHAR] (MAX) NULL,
--     Did_Attend [INT] NOT NULL,
--     --- Did this person eventually attend?
--     --- Records 0 if the person attended otherwise records 1; 
-- );
-- GO

-- Insert rows into table 'RSVP'

-- INSERT INTO RSVP
--     ( -- columns to insert data into
--     [Date_For], [Contact],
--     [_Name], [Did_Attend]
--     )
-- VALUES
--     ( -- first row: values for the columns in the list above
--         CONVERT([DATETIME], '19-02-19 9:55:56 PM', 5), 'cocoon.cocoon@gmail.com',
--         'Ugochukwu Maduka', 0
-- ),
--     ( -- second row: values for the columns in the list above
--         CONVERT([DATETIME], '12-02-19 8:15:56 PM', 5), '0700002685682',
--         'Kenny Maddy', 0
-- ),
--     ( -- third row: values for the columns in the list above
--         CONVERT([DATETIME], '11-02-19 7:23:26 PM', 5), 'kentuckymaduka@yahoo.com',
--         'Coach Oris', 1      
-- ),
--     ( -- fourth row: values for the columns in the list above
--         CONVERT([DATETIME], '13-02-19 6:51:26 PM', 5), '3040583884384',
--         'Ciroma Adekunle Chukwuma', 0
-- )
-- -- add more rows here
-- GO

-- -- Create a stored procedure
-- -- to add an RSVP to the RSVP table

-- DROP PROCEDURE dbo.spSubmitRSVP;
-- GO

-- CREATE PROCEDURE spSubmitRSVP
--     (
--     @Date_For [DATETIME],
--     @Contact [NVARCHAR] (MAX),
--     @_Name [NVARCHAR] (MAX),
--     @Did_Attend [INT]
-- )
-- AS
-- BEGIN
--     INSERT INTO RSVP
--         (

--         -- columns to insert data into

--         [Date_For], [Contact],
--         [_Name], [Did_Attend]
--         )
--     VALUES
--         (@Date_For, @Contact, @_Name,
--             @Did_Attend)
-- END

-- Create a stored procedure
-- to update an article on the
-- RSVP table.

-- CREATE PROCEDURE spUpdateRSVP
--     (
--     @ID [INT],
--     @Date_For [DATETIME],
--     @Contact [NVARCHAR] (MAX),
--     @_Name [NVARCHAR] (MAX),
--     @Did_Attend [INT]
-- )
-- AS
-- BEGIN
--     UPDATE RSVP

--  -- columns to insert updates into

--     SET [Date_For]=@Date_For, [_Name]=@_Name,
--          [Contact]=@Contact,
--          [Did_Attend]=@Did_Attend
--     WHERE ID=@ID
-- END

-- Create a stored procedure
-- To delete a record
-- from the RSVP table

-- CREATE PROCEDURE spDeleteRSVP
--     (
--     @ID [INT]
-- )
-- AS
-- BEGIN
--     DELETE FROM RSVP WHERE ID=@ID
-- END


-- Create a stored procedure to
-- retrieve all RSVP recorded on the RSVP table


-- CREATE PROCEDURE spGetAllRSVP
-- AS
-- BEGIN
--     SELECT *
--     FROM dbo.RSVP
-- END

-- Create a stored procedure to 
-- retrieve all RSVPs scheduled for a
-- particular date.

-- CREATE PROCEDURE spGetRSVPsByDate
--     (
--     @date_For [DATETIME]
-- )
-- AS
-- BEGIN
--     SELECT *
--     FROM dbo.RSVP
--     WHERE cast([Date_For] as date ) = @date_For
-- END


-- Create a stored procedure to
-- retrieve a single RSVP on the RSVP table

-- CREATE PROCEDURE spGetSingleRSVP
--     (
--     @ID [INT]
-- )
-- AS
-- BEGIN
--     SELECT *
--     FROM RSVP
--     WHERE @ID=ID
-- END


-- EXEC dbo.spGetSingleRSVP 1;
-- GO

-- DROP PROCEDURE dbo.spSubmitRSVP;
-- GO

-- DECLARE @datefor DATETIME
-- DECLARE @name NVARCHAR(MAX)
-- DECLARE @Contact NVARCHAR(MAX)
-- DECLARE @Did_Attend NVARCHAR(MAX)

-- SET @datefor = '2020-01-08T10:29:34';
-- SET @name = 'Kent2ckyDev';
-- SET @Contact = 'proper contact';
-- SET @Did_Attend = 1;

-- EXEC dbo.spUpdateRSVP 6, @datefor, @Contact, @name, @Did_Attend

-- EXEC dbo.spSubmitRSVP @datefor, @Contact, @name, @Did_Attend ;
-- GO


-- DROP PROCEDURE dbo.spSubmitRSVP;
-- GO

-- DELETE FROM RSVP WHERE ID = 2;
-- GO

-- EXEC dbo.spDeleteRSVP 6

EXEC dbo.spGetAllRSVP ;
GO

-- SELECT *
-- FROM RSVP
-- GO

--- Some of the queries contained in this script are for reference purposes
--- in case I need them in the future.