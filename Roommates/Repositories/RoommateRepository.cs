using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT FirstName, RentPortion, r.Name 
                                        FROM Roommate rm 
                                        join Room r on r.Id = rm.RoomId 
                                        WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        if(reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                }
                            };
                        }
                        return roommate;
                    }
                }
            }
        }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Roommate";

                    List<Roommate> roommates = new List<Roommate>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Roommate roomate = new Roommate
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            };

                            roommates.Add(roomate);
                        }

                        return roommates;
                    }


                }
            }
        }
    }
}

