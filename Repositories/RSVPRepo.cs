using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Inspiration_International.Entities;
using Inspiration_International.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Inspiration_International.Repositories
{
    public class RSVPRepo : IRSVPRepo
    {
        private string connectionString { get; set; }
        ILogger<RSVPRepo> _logger { get; set; }

        public RSVPRepo(IConfiguration configuration, ILogger<RSVPRepo> logger)
        {
            connectionString = "Data Source=SQL5049.site4now.net;Initial Catalog=DB_A5614F_inspintdb;User Id=DB_A5614F_inspintdb_admin;Password=ken123nis;";// configuration["Secrets:ConnectionString"];
            _logger = logger;
        }

        public RSVPRepo()
        {
        }


        public async Task<IEnumerable<RSVP>> GetAllRSVPsAsync()
        {
            _logger.LogInformation($"Getting all RSVPs.................\n");
            try
            {
                //Create a list of RSVPs
                List<RSVP> RSVPs = new List<RSVP>();

                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve the RSVPs using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetAllRSVP", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // Create rsvp objects from data retrieved from the database
                        RSVP rsvp = new RSVP();
                        rsvp.RsvpID = Convert.ToInt16(rdr["ID"]);
                        rsvp.DateFor = Convert.ToDateTime(rdr["Date_For"]);
                        rsvp.UserID = rdr["UserID"].ToString();
                        rsvp.DidAttend = rdr["Did_Attend"].ToString() == "0"; // Records 0 in database if the person attended otherwise records 1

                        // Populate the list with the rsvp objects
                        RSVPs.Add(rsvp);
                    }

                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return RSVPs;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in RSVPRepo's GetAllRSVPsAsync method.\n");
                return null;
            }

        }

        public async Task<RSVP> GetSingleRSVPByIDAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Invalid parameter supplied to RSVPRepo's GetSingleRSVPByIDAsync. Parameter Value: {id}");
                return null;
            }
            _logger.LogInformation($"Getting RSVP with id: {id}\n");
            try
            {
                // Create empty rsvp object

                var rsvp = new RSVP();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve single rsvp using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetSingleRSVP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // initialize rsvp object with data retrieved from database
                        rsvp.RsvpID = Convert.ToInt16(rdr["ID"]);
                        rsvp.DateFor = Convert.ToDateTime(rdr["Date_For"]);
                        rsvp.UserID = rdr["UserID"].ToString();
                        rsvp.DidAttend = rdr["Did_Attend"].ToString() == "0"; // Records 0 in database if the person attended otherwise records 1
                        // rsvp.DidAttend will be true if 0 is returned from database otherwise will be false
                    }
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }
                return rsvp;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in RSVPRepo's GetSingleRSVPByIDAsync method.\n");
                return null;
            }
        }

        public async Task<RSVP> GetSingleRSVPByUserIDAsync(string userID)
        {
            if (userID == string.Empty)
            {
                _logger.LogError($"Invalid parameter supplied to RSVPRepo's GetSingleRSVPByIDAsync. Parameter Value: {userID}");
                return null;
            }
            _logger.LogInformation($"Getting RSVP with user id: {userID}\n");
            try
            {
                // Create empty rsvp object

                var rsvp = new RSVP();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve single rsvp using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetSingleRSVPByUserId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userID;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // initialize rsvp object with data retrieved from database
                        rsvp.RsvpID = Convert.ToInt16(rdr["ID"]);
                        rsvp.DateFor = Convert.ToDateTime(rdr["Date_For"]);
                        rsvp.UserID = rdr["UserID"].ToString();
                        rsvp.DidAttend = rdr["Did_Attend"].ToString() == "0"; // Records 0 in database if the person attended otherwise records 1
                        // rsvp.DidAttend will be true if 0 is returned from database otherwise will be false
                    }
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }
                return rsvp;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in RSVPRepo's GetSingleRSVPByUserIDAsync method.\n");
                return null;
            }
        }

        public async Task<RSVP> GetSingleRSVPByUserIDAndDateForAsync(string userID, DateTime dateFor)
        {
            if (userID == string.Empty)
            {
                _logger.LogError($"Invalid parameter supplied to RSVPRepo's GetSingleRSVPByUserIDAndDateForAsync. Parameter Value: {userID}\t {dateFor}\n");
                return null;
            }
            _logger.LogInformation($"Getting RSVP with user id: {userID} and date: {dateFor.Date}\n");
            try
            {
                // Create empty rsvp object

                var rsvp = new RSVP();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve single rsvp using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetSingleRSVPByUserIdAndDateFor", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userID;
                    cmd.Parameters.Add("@Date_For", SqlDbType.DateTime).Value = dateFor.Date;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // initialize rsvp object with data retrieved from database
                        rsvp.RsvpID = Convert.ToInt16(rdr["ID"]);
                        rsvp.DateFor = Convert.ToDateTime(rdr["Date_For"]);
                        rsvp.UserID = rdr["UserID"].ToString();
                        rsvp.DidAttend = rdr["Did_Attend"].ToString() == "0"; // Records 0 in database if the person attended otherwise records 1
                        // rsvp.DidAttend will be true if 0 is returned from database otherwise will be false
                    }
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }
                return rsvp;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in RSVPRepo's GetSingleRSVPByUserIDAndDateForAsync method.\n");
                return null;
            }
        }


        public async Task<IEnumerable<RSVP>> GetRSVPsByDateAsync(DateTime dateFor)
        {
            // This method retrieves RSVPs from database according to the
            // date they were posted for.
            if (dateFor == null)
            {
                _logger.LogError($"Invalid parameter supplied to rsvpRepo's GetrsvpsByDateAsync. Parameter Value: {dateFor}");
                return null;
            }

            _logger.LogInformation($"Getting RSVPs with date: {dateFor}..................\n");

            try
            {
                // Create article object

                var RSVPS = new List<RSVP>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve single article using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetRSVPsByDate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@date_For", SqlDbType.DateTime).Value = dateFor;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // initialize article object with data retrieved from database
                        var rsvp = new RSVP();
                        rsvp.RsvpID = Convert.ToInt16(rdr["ID"]);
                        rsvp.DateFor = Convert.ToDateTime(rdr["Date_For"]);
                        rsvp.UserID = rdr["UserID"].ToString();
                        rsvp.DidAttend = rdr["Did_Attend"].ToString() == "0"; // Records 0 in database if the person attended otherwise records 1
                        // rsvp.DidAttend will be true if 0 is returned from database otherwise will be false

                        RSVPS.Add(rsvp);
                    }

                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }
                return RSVPS;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in RSVPRepo's GetRSVPsByDateAsync method.");
                return null;
            }
        }
        public async Task<RSVP> UpdateRSVPAsync(int rsvpID, DateTime dateFor, string userID, int didAttend)
        {

            // Throws nullReferenceException if parameter rsvpID has an invalid value
            if (rsvpID <= 0)
            {
                _logger.LogError($"Invalid parameter supplied to RSVPRepo's GetSingleRSVPByIDAsync. Parameter Value: {rsvpID}");
                return null;
            }
            _logger.LogInformation($"Updating RSVP with the id: {rsvpID} \n");
            try
            {
                int cmdResult;
                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Update rsvp row using stored procedures created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spUpdateRSVP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = rsvpID;
                    cmd.Parameters.Add("@Date_For", SqlDbType.DateTime).Value = dateFor;
                    cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userID;
                    cmd.Parameters.Add("@Did_Attend", SqlDbType.NVarChar).Value = didAttend;

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

                return cmdResult == 0 ? null : await GetSingleRSVPByIDAsync(rsvpID);
            }
            catch (Exception ex)
            {
                //Log exceptions to Logs.txt
                _logger.LogError(ex, "Something went wrong in RSVPRepo's UpdateRSVPAsync method.");
                return null;
            }
        }
        public async Task<int> DeleteRSVPAsync(int rsvpID)
        {

            // Check if parameter is 0 or a negative value. Returns 1 if true.
            if (rsvpID <= 0) { _logger.LogError($"Invalid parameter supplied. Parameter: {rsvpID}."); return 1; }

            _logger.LogInformation($"Deleting RSVP {rsvpID}.\n");
            try
            {
                int cmdresult;
                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Delete comment using stored procedures created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spDeleteRSVP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = rsvpID;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    // Execute command
                    cmdresult = await cmd.ExecuteNonQueryAsync();
                    _logger.LogInformation($"Command Executed.\t{cmdresult} rows affected.\n");
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return cmdresult == 0 ? 1 : 0; //Returns 0 if successful    
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in RSVPRepo's DeleteRSVPAsync method.");
                return 1;
            }
        }
        public async Task<int> SubmitRSVPAsync(DateTime dateFor, string userID, int didAttend)
        {

            // throws an exception if parameter (dateFor) is null
            if (dateFor == null | dateFor < DateTime.UtcNow)
            {
                throw new NullReferenceException($"Null or invalid value supplied as parameter! Parameter: dateFor = {dateFor}");
            }
            _logger.LogInformation("Submitting RSVP...................\n");
            try
            {
                int cmdresult;

                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Add rsvp using stored procedures created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spSubmitRSVP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Date_For", SqlDbType.DateTime).Value = dateFor;
                    cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userID;
                    cmd.Parameters.Add("@Did_Attend", SqlDbType.NVarChar).Value = didAttend;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    // Execute command
                    cmdresult = cmd.ExecuteNonQuery();
                    _logger.LogInformation($"Command executed!\t{cmdresult} rows affected.");
                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }

                return cmdresult == 0 ? 1 : 0; // Returns 0 if successful.
            }
            catch (Exception ex)
            {
                //Send errors to logFile.txt
                _logger.LogError(ex, "Something went wrong in RSVPRepo's SubmitRSVPAsync method.");
                return 1;
            }
        }

        public async Task<IEnumerable<(short, string, string)>> GetAllRSVPWithTheirContacts(DateTime dateFor)
        {
            // This method retrieves the contacts of those who RSVPd from database  
            // It takes the date of the class as its parameter.
            if (dateFor == null)
            {
                _logger.LogError($"Invalid parameter supplied to rsvpRepo's GetAllRSVPWithTheirContacts Parameter Value: {dateFor}");
                return null;
            }

            _logger.LogInformation($"Getting RSVPs with date: {dateFor}............\n");

            try
            {
                // Create article object

                var RSVPS = new List<(short didAttend, string email, string phoneNumber)>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Retrieve single article using a stored procedure created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spGetAllRSVPWithTheirContacts", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@date_For", SqlDbType.DateTime).Value = dateFor;

                    // Open database connection
                    await con.OpenAsync();
                    _logger.LogInformation("Database connection opened...............\n");
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    _logger.LogInformation("Reading from database...............\n");

                    while (rdr.Read())
                    {
                        // initialize article object with data retrieved from database

                        var didAttend = Convert.ToInt16(rdr["Did_Attend"]);
                        var email = rdr["Email"].ToString();
                        var phoneNumber = rdr["PhoneNumber"].ToString();

                        var contactDetails = (didAttend, email, phoneNumber);
                        RSVPS.Add(contactDetails);
                    }

                    // Close database connection
                    con.Close();
                    _logger.LogInformation("Database connection closed.\n");
                }
                return RSVPS;
            }
            catch (Exception ex)
            {
                //Send errors to Logs.txt
                _logger.LogError(ex, "Something went wrong in RSVPRepo's GetAllRSVPWithTheirContacts method.");
                return null;
            }
        }
    }
}