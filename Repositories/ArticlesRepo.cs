using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;
using Inspiration_International.Entities;
using Inspiration_International.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Inspiration_International.Repositories
{
    //
    // Summary:
    //  Represents an implementation repository of articles saved to the microsoft sql server.
    //  This class implements the IArticlesRepo interface. Please consult the 
    //  documentation of this software for more information. You can also
    //  contact the developer on twitter @k3nmaddy 
    //
    public class ArticlesRepo : IArticlesRepo
    {
        private string connectionString { get; set; }
        ILogger<ArticlesRepo> _logger { get; set; }

        public ArticlesRepo(IConfiguration configuration, ILogger<ArticlesRepo> logger)
        {
            connectionString = configuration["Secrets:InspIntConnectionString"];
            _logger = logger;
        }

        public ArticlesRepo()
        {
        }
        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            try
            {
                _logger.LogInformation("Getting all articles...............\n");
                //Create a list of Articles
                List<Article> Articles = new List<Article>();

                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve the Articles using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetAllArticles", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = cmd.ExecuteReader();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // Create article objects from data retrieved from the database
                        Article article = new Article();
                        article.ArticleID = Convert.ToInt16(rdr["Article_ID"]);
                        article.DateTimePosted = Convert.ToDateTime(rdr["Date_Posted"]);
                        article.Title = rdr["Title"].ToString();
                        article.ArticleBody = rdr["Article_Body"].ToString();
                        article.Author = rdr["Author"].ToString();

                        // Populate the list with the article objects
                        Articles.Add(article);
                    }

                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return Articles;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in ArtcleRepo's GetAllArticlesAsync method.");
                return null;
            }
        }
        public async Task<IEnumerable<Article>> GetArticlesByDatePostedAsync(DateTime date)
        {
            // This method retrieves articles from database according to the
            // date they were posted.
            if (date == null)
            {
                _logger.LogError($"Invalid parameter supplied to ArticleRepo's GetArticlesByDatePostedAsync. Parameter Value: {date}");
                return null;
            }
            _logger.LogInformation($"Getting articles posted on: {date}\n");

            try
            {
                // Create article object

                var Articles = new List<Article>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve single article using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetArticlesByDatePosted", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Date_Posted", SqlDbType.DateTime).Value = date;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = cmd.ExecuteReader();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // initialize article object with data retrieved from database
                        var article = new Article();
                        article.ArticleID = Convert.ToInt16(rdr["Article_ID"]);
                        article.DateTimePosted = Convert.ToDateTime(rdr["Date_Posted"]);
                        article.Title = rdr["Title"].ToString();
                        article.ArticleBody = rdr["Article_Body"].ToString();
                        article.Author = rdr["Author"].ToString();

                        Articles.Add(article);
                    }

                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return Articles;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in ArtcleRepo's GetArticlesByDatePostedAsync method.");
                return null;
            }

        }
        public async Task<Article> GetSingleArticleByIDAsync(int articleID)
        {
            if (articleID <= 0)
            {
                _logger.LogError($"Invalid parameter supplied to ArticlesRepo's GetSingleArticleByIDAsync. Parameter Value: {articleID}");
                return null;
            }
            _logger.LogInformation($"Getting article with id: {articleID}.\n");
            try
            {
                // Create article object

                var article = new Article();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve single article using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetSingleArticleByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = articleID;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = cmd.ExecuteReader();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // initialize article object with data retrieved from database
                        article.ArticleID = Convert.ToInt16(rdr["Article_ID"]);
                        article.DateTimePosted = Convert.ToDateTime(rdr["Date_Posted"]);
                        article.Title = rdr["Title"].ToString();
                        article.ArticleBody = rdr["Article_Body"].ToString();
                        article.Author = rdr["Author"].ToString();

                    }

                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return article;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in ArticleRepo's GetSingleArticleByIDAsync method.");
                return null;
            }
        }
        public async Task<Article> UpdateArticleAsync(int articleID, DateTime dateTimePosted, string title, string articleBody, string author)
        {
            // Throws nullReferenceException if the articleID parameter is invalid.
            if (articleID <= 0)
            {
                _logger.LogError($"Null or invalid id supplied as parameter! Parameter value: {articleID}.");
            }

            try
            {
                int cmdResult;

                // Connect to the database
                _logger.LogInformation($"Updating article with the id: {articleID} \n");

                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    // Update article using stored procedures created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spUpdateArticle", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = articleID;
                    cmd.Parameters.Add("@Date_Posted", SqlDbType.DateTime).Value = dateTimePosted;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = title;
                    cmd.Parameters.Add("@Article_Body", SqlDbType.NVarChar).Value = articleBody;
                    cmd.Parameters.Add("@Author", SqlDbType.NVarChar).Value = author;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    // Execute command
                    cmdResult = cmd.ExecuteNonQuery();
                    _logger.LogInformation($"Command executed!\t{cmdResult} rows affected.\n");

                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return cmdResult == 0 ? null : await GetSingleArticleByIDAsync(articleID);
            }
            catch (Exception ex)
            {
                //Log exceptions to Logs.txt
                _logger.LogError(ex, "Something went wrong in ArticleRepo's updateArticleAsync method.");
                return null;
            }
        }
        public async Task<int> DeleteArticleAsync(int articleID)
        {
            // Check if parameter is 0 or a negative value. Returns 1 if true.
            if (articleID <= 0) { _logger.LogError($"Invalid parameter supplied. Parameter: {articleID}."); return 1; }

            _logger.LogInformation($"Deleting article {articleID}.\n");

            try
            {
                int result;
                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {


                    // Delete article using stored procedures created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spDeleteArticle", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = articleID;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    // Execute command
                    result = await cmd.ExecuteNonQueryAsync();
                    _logger.LogInformation($"Command Executed.\t{result} rows affected.\n");
                    Console.WriteLine("Command executed!");
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return result == 0 ? 1 : 0; //Returns 0 if successful    
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in ArticleRepo's DeleteArticleAsync method.");
                return 1;
            }
        }


        public async Task<int> SubmitArticleAsync(DateTime dateTimePosted, string title, string articleBody, string author)
        {
            Helper helper = new Helper();
            // throws an exception if any of the parameters is invalid
            if (!helper.AllIsValid(dateTimePosted, title, articleBody, author) || !helper.DateIsValid(dateTimePosted))
            {
                throw new NullReferenceException($"Null or invalid value supplied as parameter!"
                + $"Params supplied: {dateTimePosted} {title} {author}");
            }
            _logger.LogInformation("Submitting Article...................\n");
            try
            {
                int result;
                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Add article using stored procedures created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spSubmitArticle", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Date_Posted", SqlDbType.DateTime).Value = dateTimePosted;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = title;
                    cmd.Parameters.Add("@Article_Body", SqlDbType.NVarChar).Value = articleBody;
                    cmd.Parameters.Add("@Author", SqlDbType.NVarChar).Value = author;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    // Execute command
                    result = cmd.ExecuteNonQuery();
                    _logger.LogInformation($"Command executed!\t{result} rows affected.");
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }
                return result == 0 ? 1 : 0; // Returns 0 if successful.
            }
            catch (Exception ex)
            {
                //Send errors to logFile.txt
                _logger.LogError(ex, "Something went wrong in ArticlesRepo's SubmitArticleAsync method.");
                return 1;
            }
        }
        public async Task<Article> GetSingleArticleWithCommentsByIDAsync(int articleID)
        {
            if (articleID <= 0)
            {
                _logger.LogError($"Invalid parameter supplied to ArticlesRepo's GetSingleArticleWithCommentsByIDAsync. Parameter Value: {articleID}");
                return null;
            }
            _logger.LogInformation($"Getting article with all its comments: {articleID}\n");
            try
            {
                // Create article object
                var article = new Article();

                // Create list of comments
                var comments = new List<Comment>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve single article using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetSingleArticleWithCommentsByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@article_id", SqlDbType.Int).Value = articleID;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = cmd.ExecuteReader();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        var comment = new Comment();
                        comment.CommentID = Convert.ToInt16(rdr["Comment_ID"]);
                        comment.DateTimePosted = Convert.ToDateTime(rdr["Date_Posted"]);
                        comment.CommentBody = rdr["Comment_Body"].ToString();
                        comment.Name = rdr["_Name"].ToString();
                        // initialize article object with data retrieved from database
                        article.ArticleID = Convert.ToInt16(rdr["Article_ID"]);
                        article.DateTimePosted = Convert.ToDateTime(rdr["Date_Posted"]);
                        article.Title = rdr["Title"].ToString();
                        article.ArticleBody = rdr["Article_Body"].ToString();
                        article.Author = rdr["Author"].ToString();

                        comments.Add(comment);
                    }
                    article.Comments = comments;
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }
                return article;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in ArtcleRepo's GetSingleArticleByIDAsync method.");
                return null;
            }
        }

        public async Task<int> DeleteSingleArticleWithCommentsByID(int articleID)
        {
            // Check if parameter is 0 or a negative value. Returns 1 if true.
            if (articleID <= 0) { _logger.LogError($"Invalid parameter supplied. Parameter: {articleID}.", articleID); return 1; }
            _logger.LogInformation($"Deleting article and all its comments {articleID}.\n");

            try
            {
                int result;
                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Delete article together with the associated comments
                    //using stored procedures created in the database

                    SqlCommand cmd = new SqlCommand("dbo.spDeleteSingleArticleWithCommentsByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@article_id", SqlDbType.Int).Value = articleID;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    // Execute command
                    result = await cmd.ExecuteNonQueryAsync();
                    _logger.LogInformation($"Command Executed.\t{result} rows affected.\n");
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return result == 0 ? 1 : 0; //Returns 0 if successful    
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in ArticleRepo's DeleteSingleArticleWithCommentsAsync method.");
                return 1;
            }
        }
    }
}