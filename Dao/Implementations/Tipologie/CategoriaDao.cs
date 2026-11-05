using project_veicoli_ado.Dao.Interface.Tipologie;
using project_veicoli_ado.Models.Tipologie;
using System.Data.SqlClient;
using System;

namespace project_veicoli_ado.Dao.Implementations.Tipologie
{
    public class CategoriaDao : ICategoriaDao
    {
        private readonly string _connectionString;

        public CategoriaDao(string connectionString) { 
            _connectionString = connectionString;
        }

        public Categoria GetById(int id)
        {
            string query = $" select c.Id, c.Nome from Categoria as c where c.Id = {id} ";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) {
                        Categoria categoria = MapCategoria(reader);
                        return categoria;
                    }
                }
            }
            throw new Exception("categoria non trovata");
        }

        public Categoria MapCategoria(SqlDataReader reader)
        {
            return new Categoria
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome"))
            };
        }
    }
}
