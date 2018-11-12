using Sitecore.TechnicalChallenge.WebApi.DbEntities;
using Sitecore.TechnicalChallenge.WebApi.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sitecore.TechnicalChallenge.WebApi.Repository
{
    public class MemberRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;

        public Member GetMemberByUsername(string username)
        {
            var result = new List<Member>();
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    const string sql =
                        "SELECT Id, Username, FirstName, LastName FROM Member WHERE Username = @username";
                    conn.Open();

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add("@username",SqlDbType.NVarChar);
                        cmd.Parameters["@username"].Value = username;

                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result.Add(new Member
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3)
                            });
                        }
                        reader.Close();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }

            return result.FirstOrDefault();
        }

        public bool ValidateLogin(string username, string password)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    const string sql =
                        "SELECT Id FROM Member WHERE Username = @username AND Password = @password";
                    conn.Open();

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add("@username", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@password", SqlDbType.NVarChar);

                        cmd.Parameters["@username"].Value = username;
                        cmd.Parameters["@password"].Value = password;

                        var result = cmd.ExecuteScalar();

                        return result != null;
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public int RegisterMember(RegistrationModel model)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    const string sql =
                        "INSERT INTO Member (Username,[Password],FirstName,LastName) VALUES(@username,@password,@firstname,@lastname)";
                    conn.Open();

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add("@username", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@password", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@firstname", SqlDbType.NVarChar);
                        cmd.Parameters.Add("@lastname", SqlDbType.NVarChar);

                        cmd.Parameters["@username"].Value = model.Username;
                        cmd.Parameters["@password"].Value = model.Password;
                        cmd.Parameters["@firstname"].Value = model.FirstName;
                        cmd.Parameters["@lastname"].Value = model.LastName;

                        return cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}