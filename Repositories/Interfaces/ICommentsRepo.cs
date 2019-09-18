using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inspiration_International.Entities;

namespace Inspiration_International.Repositories
{
    public interface ICommentsRepo
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> GetSingleCommentByIDAsync(int id);
        Task<Comment> UpdateCommentAsync(int commentID, DateTime dateTimePosted, string commentBody, string name, int articleID);
        Task<int> DeleteCommentAsync(int commentID);
        Task<int> SubmitCommentAsync(DateTime dateTimePosted, string commentBody, string name, int articleID);
    }
}