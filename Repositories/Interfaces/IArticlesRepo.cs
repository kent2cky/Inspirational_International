using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inspiration_International.Entities;

namespace Inspiration_International.Repositories
{
    public interface IArticlesRepo
    {
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<IEnumerable<Article>> GetArticlesByDatePostedAsync(DateTime date);
        Task<Article> GetSingleArticleByIDAsync(int id);
        Task<Article> UpdateArticleAsync(int id, DateTime dateTimePosted, string title, string articleBody, string author);
        Task<int> DeleteArticleAsync(int articleID);
        Task<int> SubmitArticleAsync(DateTime datePosted, string title, string articleBody, string author);
        Task<Article> GetSingleArticleWithCommentsByIDAsync(int articleID);
        Task<int> DeleteSingleArticleWithCommentsByID(int articleID);

    }
}