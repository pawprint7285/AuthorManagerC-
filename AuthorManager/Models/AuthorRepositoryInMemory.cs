using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorManager.Models
{
    public class AuthorRepositoryInMemory : IAuthorRepository
    {
        private static List<Author> _authors;
        private static int _nextId = 1;

        public AuthorRepositoryInMemory()
        {
            if(_authors == null)
            {
                _authors = new List<Author>();
            }
        }
        public void AddAuthor(Author newAuthor)
        {
            newAuthor.Id = _nextId++;
            _authors.Add(newAuthor);
        }

        public Author GetById(int id)
        {
            return _authors.Find(a => a.Id == id);
        }

        public List<Author> ListAll()
        {
            return _authors;
        }
    }
}
