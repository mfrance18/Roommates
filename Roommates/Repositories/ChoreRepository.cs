using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;
using System;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select Id, Name FROM Chore";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");

                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");

                            string nameValue = reader.GetString(nameColumnPosition);


                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            chores.Add(chore);
                        }

                        return chores;
                    }
                }
            }
        }

        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;

                        if (reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        return chore;
                    }
                }
            }
        }


        public void Insert(Chore chore)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }

        public List <Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * from Chore 
                                        Left join RoommateChore on RoommateChore.ChoreId = Chore.Id 
                                        where RoommateChore.RoommateId IS NULL";
                    List<Chore> unassignedChores = new List<Chore>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chore noAssignment = new Chore
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            unassignedChores.Add(noAssignment);
                        }

                        return unassignedChores;
                    }
                }
            }
        }

        public void AssignChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (RoommateId, ChoreId) 
                                        OUTPUT INSERTED.Id 
                                        VALUES (@roommateId, @choreId)";
                    cmd.Parameters.AddWithValue("@roommateId", roommateId);
                    cmd.Parameters.AddWithValue("@choreId", choreId);

                    cmd.ExecuteScalar();
                }
            }
        }


        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                    SET Name = @name
                                    WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }

        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Chore WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch(SqlException ex)
                    {
                        Console.WriteLine("Chore has been assigned. Cannot Be Deleted");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                    }
                    
                }
            }
        }
    }
}
