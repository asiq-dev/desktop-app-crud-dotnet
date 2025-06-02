using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;


namespace WindowsFormsApplication1
{
   public class PersonRepository
    {
        private readonly string connectionString;
        public PersonRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString;

        }

        public List<Person> GetAll()
        {
            var people = new List<Person>();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Select Id, Name, Email from Persons", connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        people.Add(
                            new Person
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString()
                            }
                        );
                    }
                }
            }
            return people;
        }


        public void Add(Person person)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Insert into Persons (Name, Email) values (@Name, @Email); Select SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Email", person.Email);

                connection.Open();
                person.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(Person person)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("UPDATE Persons SET Name = @Name, Email = @Email WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", person.Id);
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Email", person.Email);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("DELETE FROM Persons WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}
