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
        Task<IEnumerable<RSVP>> GetRSVPsByDateAsync(DateTime dateFor);
        Task<RSVP> UpdateRSVPAsync(int rsvpID, DateTime dateFor, string contact, string name, int didAttend);
        Task<int> DeleteRSVPAsync(int id);
        Task<int> SumbitRSVPAsync(DateTime dateFor, string contact, string name, int didAttend);
    }
}