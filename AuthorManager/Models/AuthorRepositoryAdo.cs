using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorManager.Models
{
    public class AuthorRepositoryAdo : IAuthorRepository
    {
        ILogger<AuthorRepositoryAdo> _log;
        private string connStr = "Server=(localdb)\\mssqllocaldb;Database=.aspnet-AuthorManager-6C8427B3-98A7-4DFF-A70F-4E2071A932BA;Trusted_Connection=True;MultipleActiveResultSets=true";

        private string selectQuery = "SELECT ID, FName, LName, Email\n"
            + "FROM Authors\n";

        private string selectByIdClause = "WHERE Id = @id\n";

        private string orderByName = "ORDER BY LName, FName\n";

        private string insertAuthorQuery = "INSERT INTO Authors\n"
            + "(FName, LName, Email)\n"
            + "values(@fname,@lname,@email";

        public AuthorRepositoryAdo(ILogger<AuthorRepositoryAdo> log)
        {
            _log = log;
        }

        public void AddAuthor(Author newAuthor)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    var command = new SqlCommand(insertAuthorQuery, conn);

                    command.Parameters.AddWithValue("fname", newAuthor.FName);
                    command.Parameters.AddWithValue("lname", newAuthor.LName);
                    command.Parameters.AddWithValue("email", newAuthor.Email);

                    conn.Open();

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                    _log.LogError(ex, "Error Adding Author {fname} {lname}", newAuthor.FName, newAuthor.LName);
                }
            }
        }

        public Author GetById(int id)
        {
            Author author = new Author();

            using(var conn = new SqlConnection(connStr))
            {
                SqlCommand command = new SqlCommand(selectQuery + selectByIdClause, conn);

                command.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        author.Id = int.Parse(reader[0].ToString());
                        author.FName = reader[1].ToString();
                        author.LName = reader[2].ToString();
                        author.Email = reader[3].ToString();
                    };
 
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "Error reading author by ID for ID: {id}", id);
                }
            }

            return author;
        }

        public List<Author> ListAll()
        {
            List<Author> authors = new List<Author>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand command = new SqlCommand(selectQuery + orderByName, conn);

                try
                {
                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //var foo = reader.GetSqlInt64(0);
                        Author newAuthor = new Author
                        {
                            Id = int.Parse(reader[0].ToString()),
                            FName = reader[1].ToString(),
                            LName = reader[2].ToString(),
                            Email = reader[3].ToString()
                        };

                        authors.Add(newAuthor);
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "Error loading list of Authors.");
                }
            }

            return authors;
        }
    }
}
