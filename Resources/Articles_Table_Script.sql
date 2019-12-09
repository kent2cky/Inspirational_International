-- ---- README README README README README README README     -----
-- ---- This is a script used to create and manage the       -----
-- ---- Articles table. The script in itself is self----
-- ---- explaining. However it is well commented so that who- ----
-- ---- ever reads it will understand what is going on here. -----
-- ---- The scripts are commented out by default. Please     -----
-- ---- uncomment just the part you want to work with. You can ---
-- ---- execute that part individually and others will not   -----
-- ---- interfere.                                           -----
-- -----
-- ---- Goodluck!                                            -----
-- ---- README README README README README README README     -----

-- ---- I'm keeping some of the queries here as reminders
-- ---- in the future.

-- SELECT name, database_id, create_date
-- FROM sys.databases ;
-- GO

-- -- DROP DATABASE Transactions;
-- -- Go

-- -- CREATE DATABASE InspInt_Database;
-- -- GO

-- -- SELECT *
-- -- FROM INFORMATION_SCHEMA.TABLES
-- -- GO

-- -- Create a new table called 'Articles' in schema 'dbo'
-- -- Drop the table if it already exists

-- IF OBJECT_ID('dbo.Articles', 'U') IS NOT NULL
-- DROP TABLE dbo.Articles
-- GO

-- -- Create the table in the specified schema

-- CREATE TABLE dbo.Articles
-- (
--     Article_ID INT NOT NULL IDENTITY PRIMARY KEY,
--     -- primary key column
--     Date_Posted [DATETIME] NOT NULL,
--     Title [NVARCHAR] (MAX) NOT NULL,
--     Article_Body [NVARCHAR] (MAX) NOT NULL,
--     Author [NVARCHAR] (MAX) NOT NULL,
-- );
-- GO

-- -- Insert rows into table 'Articles'
-- -- Forgive me for the long contents.

-- INSERT INTO Articles
--     ( -- columns to insert data into
--     [Date_Posted], [Title],
--     [Article_Body], [Author]
--     )
-- VALUES
--     ( -- first row: values for the columns in the list above
--         CONVERT([DATETIME], '10-02-19 9:55:56 PM', 5), 'Where there is a way',
--         'Lorem ipsum Outer Joins. Outer joins are used to match rows from two tables.  Even
-- if there is no match rows are included.  Rows from one of the tables are always included, 
-- for the other, when there are no matches, NULL values are included.The series starts
-- with the article Introduction to Database Joins.  All the examples for this lesson are based on 
-- Microsoft SQL Server Management Studio and the AdventureWorks2012 database.  You can get started using 
-- these free tools using my Guide Getting Started Using SQL Server. In this article we are going to cover outer joins.
-- Types of Outer Joins There are three types of outer joins:Left Outer Join – All rows from the left 
-- table are included, unmatched rows from the right are replaced
-- with NULL values.Right Outer Join – All rows from the right table are included, 
-- unmatched rows from the left are replaced
-- with NULL values.Full Outer Join – All rows from both tables are included, 
-- NULL values fill unmatched rows.Let’s dig a deeper and explore the left outer join.',
--         'Kris Wenzel'
-- ),
--     ( -- second row: values for the columns in the list above
--         CONVERT([DATETIME], '11-02-19 8:15:56 PM', 5), 'A Random Title', 'The model
-- is shown
-- below:

-- Full Outer Join Model

-- Suppose we want to know all the currencies we can place orders in and which orders were placed in those currencies?

-- SELECT sales.SalesOrderHeader.AccountNumber,
--     sales.SalesOrderHeader.OrderDate,
--     sales.CurrencyRate.ToCurrencyCode,
--     sales.CurrencyRate.AverageRate
-- FROM sales.SalesOrderHeader
--     FULL OUTER JOIN
--     sales.CurrencyRate
--     ON sales.CurrencyRate.CurrencyRateID = 
--           sales.SalesOrderHeader.CurrencyRateID

-- Here is a portion of the results showing where some sales have match to a currency and some that haven’t.  The reason there are sales that don’t match is that these are sales in USD.

-- Full Outer Join Results 1

-- Further down in the results you see currencies
-- with no matching sales.  This reflects the fact that no sales were made in those currencies.

-- Full Outerjoins 2

-- Note:
-- I was surprised to see USD listed, see row 42463, since I would think a majority of the sales would be in this currency.  My thought is that rather than reverence the currency rate for these transaction, the SalesOrderHeader vale for CurrencyRateID was
-- set
-- to null for all USD transactions.  I think this is inconsistent, and isn’t the way I would do it, but it isn’t my database…
-- Advanced Example

-- So far we’ve looked at the three types of outer joins but haven’t explored some more advanced concepts such as joining multiple table and using more than one condition in our join clauses.

-- We covered these concepts when we explored inner joins, so what I’ll be showing you, shouldn’t be too new, but I think it still makes sense to review, since in some cases mixing full joins
-- with inner joins may produce unexpected or unintended results.

-- Let’s turn our focus to the production schema and explore products and categories.  Let’s produce a list of all product categories and the product models contained within.

-- Product has a one to many relationship
-- with ProductModel and ProductSubcategory.  Since it lies between these two tables, there is an implicit many to many relationship between ProductModel and ProductSubcategory.   Because of this, it is a good candidate for outer joins as there is may be product models
-- with no assigned products and ProductSubcategory entries
-- with no product.

-- Product Category Datamodel

-- To overcome this situation we will do an outer join to both the ProductModel and ProductCategory table.

-- Here is the SQL

-- SELECT PC.Name AS Category,
--     PSC.Name AS Subcategory,
--     PM.Name AS Model,
--     P.Name AS Product
-- FROM Production.Product AS P
--     FULL OUTER JOIN
--     Production.ProductModel AS PM
--     ON PM.ProductModelID = P.ProductModelID
--     FULL OUTER JOIN
--     Production.ProductSubcategory AS PSC
--     ON PSC.ProductSubcategoryID = P.ProductSubcategoryID
--     INNER JOIN
--     Production.ProductCategory AS PC
--     ON PC.ProductCategoryID = PSC.ProductCategoryID
-- ORDER BY PC.Name, PSC.Name

-- There are several
-- items to
-- note:

-- I used table aliases to make the SQL more readable.
--     There is more than one full outer join clause.
--     The ProductCategory table is also part of an outer join

-- Originally when I wrote the SQL for this query I had an inner join between ProductSubcategory and ProductCategory, but I wasn’t seeing NULL values for unmatched records I would expect.

-- Once I changed the join to a full outer join I saw the results I expected.  The reason this occurs is subtle.

-- After checking the data I confirmed that all categories are assigned subcategories.  Given this you would think an inner join would work; however, consider that as the entire statement is executed and rows are returned, the ProductSubcategoryID value is NULL whenever a product fails to match a product subcategory.

-- Null values, by definition, aren’t equal to one another, so the inner join fails.  Given this, when these values are then matched to ProductCategory they aren’t included in the result unless the join to ProductCategory is an outer join.

-- In fact, the join doesn’t have to be a full outer join, a left join works just as
-- well:

-- SELECT PC.Name AS Category,
--     PSC.Name AS Subcategory,
--     PM.Name AS Model,
--     P.Name AS Product
-- FROM Production.Product AS P
--     FULL OUTER JOIN
--     Production.ProductModel AS PM
--     ON PM.ProductModelID = P.ProductModelID
--     FULL OUTER JOIN
--     Production.ProductSubcategory AS PSC
--     ON PSC.ProductSubcategoryID = P.ProductSubcategoryID
--     LEFT OUTER JOIN
--     Production.ProductCategory AS PC
--     ON PC.ProductCategoryID = PSC.ProductCategoryID
-- ORDER BY PC.Name, PSC.Name

-- Uses for Outer Joins


-- Because outer joins not only the matching rows but also those that don’t, they are a really good way to find missing entries in tables.  This are great when you need to do diagnosis on your database to determine
-- if there are data integrity issues.

-- For example suppose we were concerned that we may have some ProductSubcategory entries that don’t match to Categories.  We could test by running the following SQL

-- SELECT PSC.Name AS Subcategory
-- FROM Production.ProductCategory AS PSC
--     LEFT OUTER JOIN
--     Production.ProductSubcategory AS PC
--     ON PC.ProductCategoryID = PSC.ProductCategoryID
-- WHERE PSC.ProductCategoryID is NULL

-- The outer join returns the unmatched row values as NULL values.  The where clause filters on the non-null values, leaving only nonmatching Subcategory names for us to review.

-- Outer joins can also be used to ask questions such
-- as:

-- “What sales persons have never made a sale?”

-- “What products aren’t assigned to a product model?”

-- “Which departments don’t have any assigned employees?”

-- “List all sales territories not assigned sales people.”',
--         'Kennis Maduka'
-- ),
--     ( -- third row: values for the columns in the list above
--         CONVERT([DATETIME], '12-02-19 7:23:26 PM', 5), 'Another Random Title',
--         'The model
-- is shown
-- below:

-- Full Outer Join Model

-- Suppose we want to know all the currencies we can place orders in and which orders were placed in those currencies?

-- SELECT sales.SalesOrderHeader.AccountNumber,
--     sales.SalesOrderHeader.OrderDate,
--     sales.CurrencyRate.ToCurrencyCode,
--     sales.CurrencyRate.AverageRate
-- FROM sales.SalesOrderHeader
--     FULL OUTER JOIN
--     sales.CurrencyRate
--     ON sales.CurrencyRate.CurrencyRateID = 
--           sales.SalesOrderHeader.CurrencyRateID

-- Here is a portion of the results showing where some sales have match to a currency and some that haven’t.  The reason there are sales that don’t match is that these are sales in USD.

-- Full Outer Join Results 1

-- Further down in the results you see currencies
-- with no matching sales.  This reflects the fact that no sales were made in those currencies.

-- Full Outerjoins 2

-- Note:
-- I was surprised to see USD listed, see row 42463, since I would think a majority of the sales would be in this currency.  My thought is that rather than reverence the currency rate for these transaction, the SalesOrderHeader vale for CurrencyRateID was
-- set
-- to null for all USD transactions.  I think this is inconsistent, and isn’t the way I would do it, but it isn’t my database…
-- Advanced Example

-- So far we’ve looked at the three types of outer joins but haven’t explored some more advanced concepts such as joining multiple table and using more than one condition in our join clauses.

-- We covered these concepts when we explored inner joins, so what I’ll be showing you, shouldn’t be too new, but I think it still makes sense to review, since in some cases mixing full joins
-- with inner joins may produce unexpected or unintended results.

-- Let’s turn our focus to the production schema and explore products and categories.  Let’s produce a list of all product categories and the product models contained within.

-- Product has a one to many relationship
-- with ProductModel and ProductSubcategory.  Since it lies between these two tables, there is an implicit many to many relationship between ProductModel and ProductSubcategory.   Because of this, it is a good candidate for outer joins as there is may be product models
-- with no assigned products and ProductSubcategory entries
-- with no product.

-- Product Category Datamodel

-- To overcome this situation we will do an outer join to both the ProductModel and ProductCategory table.

-- Here is the SQL

-- SELECT PC.Name AS Category,
--     PSC.Name AS Subcategory,
--     PM.Name AS Model,
--     P.Name AS Product
-- FROM Production.Product AS P
--     FULL OUTER JOIN
--     Production.ProductModel AS PM
--     ON PM.ProductModelID = P.ProductModelID
--     FULL OUTER JOIN
--     Production.ProductSubcategory AS PSC
--     ON PSC.ProductSubcategoryID = P.ProductSubcategoryID
--     INNER JOIN
--     Production.ProductCategory AS PC
--     ON PC.ProductCategoryID = PSC.ProductCategoryID
-- ORDER BY PC.Name, PSC.Name

-- There are several
-- items to
-- note:

-- I used table aliases to make the SQL more readable.
--     There is more than one full outer join clause.
--     The ProductCategory table is also part of an outer join

-- Originally when I wrote the SQL for this query I had an inner join between ProductSubcategory and ProductCategory, but I wasn’t seeing NULL values for unmatched records I would expect.

-- Once I changed the join to a full outer join I saw the results I expected.  The reason this occurs is subtle.

-- After checking the data I confirmed that all categories are assigned subcategories.  Given this you would think an inner join would work; however, consider that as the entire statement is executed and rows are returned, the ProductSubcategoryID value is NULL whenever a product fails to match a product subcategory.

-- Null values, by definition, aren’t equal to one another, so the inner join fails.  Given this, when these values are then matched to ProductCategory they aren’t included in the result unless the join to ProductCategory is an outer join.

-- In fact, the join doesn’t have to be a full outer join, a left join works just as
-- well:

-- SELECT PC.Name AS Category,
--     PSC.Name AS Subcategory,
--     PM.Name AS Model,
--     P.Name AS Product
-- FROM Production.Product AS P
--     FULL OUTER JOIN
--     Production.ProductModel AS PM
--     ON PM.ProductModelID = P.ProductModelID
--     FULL OUTER JOIN
--     Production.ProductSubcategory AS PSC
--     ON PSC.ProductSubcategoryID = P.ProductSubcategoryID
--     LEFT OUTER JOIN
--     Production.ProductCategory AS PC
--     ON PC.ProductCategoryID = PSC.ProductCategoryID
-- ORDER BY PC.Name, PSC.Name

-- Uses for Outer Joins

-- Because outer joins not only the matching rows but also those that don’t, they are a really good way to find missing entries in tables.  This are great when you need to do diagnosis on your database to determine
-- if there are data integrity issues.

-- For example suppose we were concerned that we may have some ProductSubcategory entries that don’t match to Categories.  We could test by running the following SQL

-- SELECT PSC.Name AS Subcategory
-- FROM Production.ProductCategory AS PSC
--     LEFT OUTER JOIN
--     Production.ProductSubcategory AS PC
--     ON PC.ProductCategoryID = PSC.ProductCategoryID
-- WHERE PSC.ProductCategoryID is NULL

-- The outer join returns the unmatched row values as NULL values.  The where clause filters on the non-null values, leaving only nonmatching Subcategory names for us to review.

-- Outer joins can also be used to ask questions such
-- as:

-- “What sales persons have never made a sale?”

-- “What products aren’t assigned to a product model?”

-- “Which departments don’t have any assigned employees?”

-- “List all sales territories not assigned sales people.”',
--         'Author Name'
-- ),
--     ( -- fourth row: values for the columns in the list above
--         CONVERT([DATETIME], '13-02-19 6:51:26 PM', 5), 'A Very Random Title',
--         'Unable
-- to connect

-- Firefox can’t establish a connection to the server at
-- localhost:
-- 5001.

--     The site could be temporarily unavailable or too busy. 
--     Try again in a few moments.
-- If you are unable to load any pages, 
-- check your computer’s network connection.
-- If your computer or network is protected by a firewall or proxy, 
-- make sure that Firefox is permitted to access the Web.',
--         'Mozilla Firefox'
-- )
-- -- add more rows here
-- GO

-- -- -- Create a stored procedure
-- -- -- to add an Article to the Articles table

-- -- DROP PROCEDURE dbo.spSubmitArticle;
-- -- GO

-- CREATE PROCEDURE spSubmitArticle
--     (
--     @Date_Posted [DATETIME],
--     @Title [NVARCHAR] (MAX),
--     @Article_Body [NVARCHAR] (MAX),
--     @Author [NVARCHAR] (MAX)
-- )
-- AS
-- BEGIN
--     INSERT INTO Articles
--         (

--         -- columns to insert data into

--         [Date_Posted], [Title],
--         [Article_Body], [Author]
--         )
--     VALUES
--         (@Date_Posted, @Title, @Article_Body,
--             @Author)
-- END

-- -- Create a stored procedure
-- -- to update an article on the
-- -- Articles table.


-- CREATE PROCEDURE spUpdateArticle
--     (
--     @ID [INT],
--     @Date_Posted [DATETIME],
--     @Title [NVARCHAR] (MAX),
--     @Article_Body [NVARCHAR] (MAX),
--     @Author [NVARCHAR](MAX)
-- )
-- AS
-- BEGIN
--     UPDATE Articles

--  -- columns to insert updates into

--     SET [Date_Posted]=@Date_Posted, [Title]=@Title,
--          [Article_Body]=@Article_Body,
--          [Author]=@Author
--     WHERE Article_ID=@ID
-- END

-- -- Create a stored procedure
-- -- To delete a Article
-- -- from the Articles table


-- CREATE PROCEDURE spDeleteArticle
--     (
--     @ID [INT]
-- )
-- AS
-- BEGIN
--     DELETE FROM Articles WHERE Article_ID=@ID
-- END


-- -- Create a stored procedure to
-- -- retrieve all Articles recorded on the Articles table

-- CREATE PROCEDURE spGetAllArticles
-- AS
-- BEGIN
--     SELECT *
--     FROM Articles
-- END

-- -- Create a stored procedure to
-- -- retrieve a single Article on the Articles table

-- CREATE PROCEDURE spGetSingleArticleByID
--     (
--     @ID [INT]
-- )
-- AS
-- BEGIN
--     SELECT *
--     FROM Articles
--     WHERE Article_ID=@ID
-- END


-- CREATE PROCEDURE spGetArticlesByDatePosted
--     (
--     @Date_Posted [DATETIME]
-- )
-- AS
-- BEGIN
--     SELECT *
--     FROM Articles
--     WHERE cast
-- ([Date_Posted] as date ) = @date_Posted
-- END


--- Create a stored procedure to retrieve a single Article
--- by the article's ID with all the comments associated with it

-- CREATE PROCEDURE spGetSingleArticleWithCommentsByID
--     (
--     @article_id [INT]
-- )
-- AS
-- BEGIN
--     SELECT dbo.Articles.Article_ID, dbo.Articles.Date_Posted, dbo.Articles.Author,
--         dbo.Articles.Title, dbo.Articles.Article_Body,
--         dbo.Comments.Comment_ID,
--         dbo.Comments.Date_Time, dbo.Comments._Name,
--         dbo.Comments.Comment_Body
--     FROM dbo.Articles
--         INNER JOIN
--         dbo.Comments
--         ON Comments.Article_ID = dbo.Articles.Article_ID
--     WHERE dbo.Articles.Article_ID = @article_id
-- END
-- GO

-- CREATE PROCEDURE spDeleteSingleArticleWithCommentsByID
--     (
--     @article_id [INT]
-- )
-- AS
-- BEGIN
--     DELETE FROM dbo.Articles
--     FROM dbo.Articles
--         INNER JOIN
--         dbo.Comments
--         ON Comments.Article_ID = dbo.Articles.Article_ID
--     WHERE dbo.Articles.Article_ID = @article_id
-- END
-- GO

--- Creat a stored procedure to retrieve a single Article
--- by the dateTime the article was posted and with all the comments associated with it

-- CREATE PROCEDURE spGetSingleArticleWithCommentsByDate
--     (
--     @Date_Posted [DATETIME]
-- )
-- AS
-- BEGIN
--     SELECT dbo.Articles.Article_ID AS Article_ID, dbo.Articles.Date_Posted, dbo.Articles.Author,
--         dbo.Articles.Title, dbo.Articles.Article_Body,
--         dbo.Comments.Date_Time, dbo.Comments._Name,
--         dbo.Comments.Comment_Body
--     FROM dbo.Articles
--         INNER JOIN
--         dbo.Comments
--         ON Comments.Article_ID = dbo.Articles.Article_ID
--     WHERE dbo.Articles.Date_Posted = @Date_Posted
-- END
-- GO


--- This area contains all the queries that were helpful
--- when I was creating this script. I am leaving them
--- for future references. 


--DROP PROCEDURE dbo.spGetSingleArticleWithCommentsByID


-- EXEC spGetSingleArticleWithCommentsByDate  '2019-02-12T19:23:26.000' --'2019-08-08T10:29:34';
-- GO


-- EXEC dbo.spDeleteArticle 6;
-- GO

-- -- DROP PROCEDURE dbo.spSubmitArticle;
-- -- GO

-- DECLARE @date DATETIME
-- DECLARE @body NVARCHAR(MAX)
-- DECLARE @author NVARCHAR(MAX)
-- DECLARE @title NVARCHAR(MAX)

-- SET @date = '2019-05-08T10:29:34';
-- SET @author = 'JoshuaDiEzigboteFunny';
-- SET @title = 'An Interesting Title';
-- SET @body = 'Football is such a driving factor towards the gate and
-- almost made it to the finish line but swimming is very hard and it is 
-- usually hard to code without a good IDE that helps you sing to the lord
-- while damning the lecturer who is teaching you how to not learn.';

-- EXEC dbo.spUpdateArticle 1, @date, @title, @body, @author

-- EXEC dbo.spSubmitArticle @date, @title, @body, @author;
-- GO

-- EXEC dbo.spGetSingleArticleWithCommentsByID 1;
-- GO

-- DROP PROCEDURE dbo.spGetSingleArticleWithCommentsByID;
-- GO

-- -- DELETE FROM Articles WHERE ID = 2;
-- -- GO
-- EXEC spDeleteSingleArticleWithCommentsByID 7;
-- -- EXEC dbo.spDeleteArticle 11

EXEC dbo.spGetAllArticles ;
GO


-- --- Some of the queries contained in this script are for reference purposes
-- --- in case I need them in the future.

