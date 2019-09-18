using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Inspiration_International.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Inspiration_International.Helpers;

namespace Inspiration_International.Repositories
{

    public class CommentsRepo : ICommentsRepo
    {
        private string connectionString { get; set; }
        ILogger<CommentsRepo> _logger { get; set; }

        public CommentsRepo(IConfiguration configuration, ILogger<CommentsRepo> logger)
        {
            connectionString = configuration["Secrets:InspIntConnectionString"];
            _logger = logger;
        }

        public CommentsRepo()
        {
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            _logger.LogInformation($"Getting all Comments.................\n");
            try
            {
                //Create a list of Comments
                List<Comment> Comments = new List<Comment>();

                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve the Comments using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetAllComments", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // Create comment objects from data retrieved from the database
                        Comment comment = new Comment();
                        comment.CommentID = Convert.ToInt16(rdr["Comment_ID"]);
                        comment.DateTimePosted = Convert.ToDateTime(rdr["Date_Time"]);
                        comment.CommentBody = rdr["Comment_Body"].ToString();
                        comment.Name = rdr["_Name"].ToString();
                        comment.ArticleID = int.Parse(rdr["Article_ID"].ToString());

                        // Populate the list with the comment objects
                        Comments.Add(comment);
                    }

                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return Comments;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in CommentRepo's GetAllCommentsAsync method.\n");
                return null;
            }
        }

        public async Task<Comment> GetSingleCommentByIDAsync(int commentID)
        {
            if (commentID <= 0)
            {
                _logger.LogError($"Invalid parameter supplied to CommentRepo's GetSingleCommentByIDAsync. Parameter Value: {commentID}");
                return null;
            }
            _logger.LogInformation($"Getting Comment with id: {commentID}\n");
            try
            {
                // Create empty comment object

                var comment = new Comment();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve single comment using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetSingleComment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Comment_ID", SqlDbType.Int).Value = commentID;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // initialize comment object with data retrieved from database
                        comment.CommentID = Convert.ToInt16(rdr["Comment_ID"]);
                        comment.DateTimePosted = Convert.ToDateTime(rdr["Date_Time"]);
                        comment.CommentBody = rdr["Comment_Body"].ToString();
                        comment.Name = rdr["_Name"].ToString();
                        comment.ArticleID = int.Parse(rdr["Article_ID"].ToString());
                    }
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }
                return comment;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in CommentRepo's GetSingleCommentByIDAsync method.\n");
                return null;
            }
        }

        public async Task<Comment> UpdateCommentAsync(int commentID, DateTime dateTimePosted, string commentBody, string name, int articleID)
        {
            // Throws nullReferenceException if parameter commentID has an invalid value
            if (commentID <= 0)
            {
                _logger.LogError($"Invalid parameter supplied to CommentRepo's GetSingleCommentByIDAsync. Parameter Value: {commentID}");
                return null;
            }
            _logger.LogInformation($"Updating Comment with the id: {commentID} \n");
            try
            {
                int cmdResult;
                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Update comment row using stored procedures created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spUpdateComment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Comment_ID", SqlDbType.Int).Value = commentID;
                    cmd.Parameters.Add("@Date_Time", SqlDbType.DateTime).Value = dateTimePosted;
                    cmd.Parameters.Add("@Comment_Body", SqlDbType.NVarChar).Value = commentBody;
                    cmd.Parameters.Add("@_Name", SqlDbType.NVarChar).Value = name;
                    cmd.Parameters.Add("@Article_ID", SqlDbType.Int).Value = articleID;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    // Execute command
                    cmdResult = await cmd.ExecuteNonQueryAsync();
                    _logger.LogInformation($"Command executed!\t{cmdResult} rows affected.\n");
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return cmdResult == 0 ? null : await GetSingleCommentByIDAsync(commentID);
            }
            catch (Exception ex)
            {
                //Log exceptions to Logs.txt
                _logger.LogError(ex, "Something went wrong in CommentRepo's UpdateCommentAsync method.");
                return null;
            }
        }

        public async Task<int> DeleteCommentAsync(int commentID)
        {

            // Check if parameter is 0 or a negative value. Returns 1 if true.
            if (commentID <= 0) { _logger.LogError($"Invalid parameter supplied. Parameter: {commentID}."); return 1; }

            _logger.LogInformation($"Deleting Comment {commentID}.\n");
            try
            {
                int result;
                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Delete comment using stored procedures created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spDeleteComment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Comment_ID ", SqlDbType.Int).Value = commentID;

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
                _logger.LogError(ex, "Something went wrong in CommentRepo's DeleteCommentAsync method.");
                return 1;
            }
        }

        public async Task<int> SubmitCommentAsync(DateTime dateTimePosted, string commentBody, string name, int articleID)
        {
            var helper = new Helper();
            // throws an exception if parameter (dateFor) is null
            if (!helper.DateIsValid(dateTimePosted))
            {
                throw new NullReferenceException($"Null or invalid value supplied as parameter! Parameter: dateTimePosted = {dateTimePosted}");
            }
            _logger.LogInformation("Submitting Comment...................\n");
            try
            {
                int result;

                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Add rsvp using stored procedures created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spSubmitComment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Date_Time", SqlDbType.DateTime).Value = dateTimePosted;
                    cmd.Parameters.Add("@Comment_Body", SqlDbType.NVarChar).Value = commentBody;
                    cmd.Parameters.Add("@_Name", SqlDbType.NVarChar).Value = name;
                    cmd.Parameters.Add("@Article_ID", SqlDbType.Int).Value = articleID; // Note that articleID is a foreign key
                    // therefore it must exist as a primary key in articles table

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
                _logger.LogError(ex, "Something went wrong in CommentRepo's SubmitCommentAsync method.");
                return 1;
            }
        }
    }


}