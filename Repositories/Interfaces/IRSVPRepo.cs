using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inspiration_International.Entities;

namespace Inspiration_International.Repositories
{
    public interface IRSVPRepo
    {
        Task<IEnumerable<RSVP>> GetAllRSVPsAsync();

        Task<RSVP> GetSingleRSVPByIDAsync(int id);
        Task<RSVP> GetSingleRSVPByUserIDAsync(string userID);
        Task<RSVP> GetSingleRSVPByUserIDAndDateForAsync(string userID, DateTime dateFor);
        Task<IEnumerable<RSVP>> GetRSVPsByDateAsync(DateTime dateFor);
        Task<RSVP> UpdateRSVPAsync(int rsvpID, DateTime dateFor, string userID, int didAttend);
        Task<int> DeleteRSVPAsync(int id);
        Task<int> SubmitRSVPAsync(DateTime dateFor, string userID, int didAttend);
        Task<IEnumerable<(short, string, string)>> GetAllRSVPWithTheirContacts(DateTime dateFor);
    }
}