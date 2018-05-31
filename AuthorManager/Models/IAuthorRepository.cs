using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorManager.Models
{
    public interface IAuthorRepository
    {
        void AddAuthor(Author newAuthor);
        Author GetById(int id);
        List<Author> ListAll();
    }
}
