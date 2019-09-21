using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Inspiration_International.Entities;
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
            connectionString = configuration["Secrets:InspIntConnectionString"];
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
                        rsvp.Contact = rdr["Contact"].ToString();
                        rsvp.Name = rdr["_Name"].ToString();
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
                        rsvp.Contact = rdr["Contact"].ToString();
                        rsvp.Name = rdr["_Name"].ToString();
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
        public async Task<IEnumerable<RSVP>> GetRSVPsByDateAsync(DateTime dateFor)
        {
            // This method retrieves RSVPs from database according to the
            // date they were posted for.
            if (dateFor == null)
            {
                _logger.LogError($"Invalid parameter supplied to rsvpRepo's GetrsvpsByDateAsync. Parameter Value: {dateFor}");
                return null;
            }

            _logger.LogInformation($"Getting RSVPs with date: {dateFor}.\n");

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
                        rsvp.Contact = rdr["Contact"].ToString();
                        rsvp.Name = rdr["_Name"].ToString();
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
        public async Task<RSVP> UpdateRSVPAsync(int rsvpID, DateTime dateFor, string contact, string name, int didAttend)
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
                    cmd.Parameters.Add("@Contact", SqlDbType.NVarChar).Value = contact;
                    cmd.Parameters.Add("@_Name", SqlDbType.NVarChar).Value = name;
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
                int result;
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
                _logger.LogError(ex, "Something went wrong in RSVPRepo's DeleteRSVPAsync method.");
                return 1;
            }
        }
        public async Task<int> SumbitRSVPAsync(DateTime dateFor, string contact, string name, int didAttend)
        {

            // throws an exception if parameter (dateFor) is null
            if (dateFor == null | dateFor < DateTime.UtcNow)
            {
                throw new NullReferenceException($"Null or invalid value supplied as parameter! Parameter: dateFor = {dateFor}");
            }
            _logger.LogInformation("Submitting RSVP...................\n");
            try
            {
                int result;

                // Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Add rsvp using stored procedures created in the database
                    SqlCommand cmd = new SqlCommand("dbo.spSubmitRSVP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Date_For", SqlDbType.DateTime).Value = dateFor;
                    cmd.Parameters.Add("@Contact", SqlDbType.NVarChar).Value = contact;
                    cmd.Parameters.Add("@_Name", SqlDbType.NVarChar).Value = name;
                    cmd.Parameters.Add("@Did_Attend", SqlDbType.NVarChar).Value = didAttend;

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
                _logger.LogError(ex, "Something went wrong in RSVPRepo's SubmitRSVPAsync method.");
                return 1;
            }
        }
    }
}