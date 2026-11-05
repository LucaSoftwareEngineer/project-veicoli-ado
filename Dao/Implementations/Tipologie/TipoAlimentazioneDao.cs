using project_veicoli_ado.Dao.Interface.Tipologie;
using project_veicoli_ado.Models.Tipologie;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace project_veicoli_ado.Dao.Implementations.Tipologie
{
    public class TipoAlimentazioneDao : ITipoAlimentazioneDao
    {
        private readonly string _connectionString;

        public TipoAlimentazioneDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TipoAlimentazione GetById(int id)
        {
            string query = $" select t.Id, t.Nome from TipoAlimentazione as t where t.Id = {id} ";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader()) {
                    if (reader.Read()) {
                        return MapTipoAlimentazione(reader);
                    }
                    throw new ArgumentNullException("Tipo di alimentazione non trovato");
                }
            }
        }

        public TipoAlimentazione MapTipoAlimentazione(SqlDataReader reader)
        {
            return new TipoAlimentazione
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome"))
            };
        }
    }
}
