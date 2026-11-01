using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using project_veicoli_ado.Dao.Interface;
using project_veicoli_ado.Models;

namespace project_veicoli_ado.Dao.Implementations
{
    public class MacchinaDao : IMacchinaDao
    {
        private readonly string _connectionString;

        public MacchinaDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Macchina> GetAll()
        {
            var lista = new List<Macchina>();
            string query = @"
                SELECT ma.MacchinaId, ma.VeicoloId, ma.Targa, ma.Cc, ma.NumeroPorte,
                       v.NumeroRuote, v.TipoAlimentazioneId, v.CategoriaId, v.Colore, v.Marca, v.AnnoProduzione, v.Modello
                FROM Macchina ma
                INNER JOIN Veicolo v ON ma.VeicoloId = v.VeicoloId";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MapMacchina(reader));
                    }
                }
            }
            return lista;
        }

        public Macchina GetById(int id)
        {
            Macchina macchina = null;
            string query = @"
                SELECT ma.MacchinaId, ma.VeicoloId, ma.Targa, ma.Cc, ma.NumeroPorte,
                       v.NumeroRuote, v.TipoAlimentazioneId, v.CategoriaId, v.Colore, v.Marca, v.AnnoProduzione, v.Modello
                FROM Macchina ma
                INNER JOIN Veicolo v ON ma.VeicoloId = v.VeicoloId
                WHERE ma.MacchinaId = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        macchina = MapMacchina(reader);
                    }
                }
            }
            return macchina;
        }

        public void Insert(Macchina macchina)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string queryVeicolo = @"
                            INSERT INTO Veicolo (NumeroRuote, TipoAlimentazioneId, CategoriaId, Colore, Marca, AnnoProduzione, Modello)
                            VALUES (@NumeroRuote, @TipoAlimentazioneId, @CategoriaId, @Colore, @Marca, @AnnoProduzione, @Modello);
                            SELECT SCOPE_IDENTITY();";

                        int veicoloId;
                        using (var cmdVeicolo = new SqlCommand(queryVeicolo, connection, transaction))
                        {
                            cmdVeicolo.Parameters.AddWithValue("@NumeroRuote", macchina.NumeroRuote);
                            cmdVeicolo.Parameters.AddWithValue("@TipoAlimentazioneId", macchina.TipoAlimentazioneId);
                            cmdVeicolo.Parameters.AddWithValue("@CategoriaId", macchina.CategoriaId);
                            cmdVeicolo.Parameters.AddWithValue("@Colore", (object)macchina.Colore ?? DBNull.Value);
                            cmdVeicolo.Parameters.AddWithValue("@Marca", macchina.Marca);
                            cmdVeicolo.Parameters.AddWithValue("@AnnoProduzione", macchina.AnnoProduzione);
                            cmdVeicolo.Parameters.AddWithValue("@Modello", macchina.Modello);

                            veicoloId = Convert.ToInt32(cmdVeicolo.ExecuteScalar());
                        }

                        string queryMacchina = @"
                            INSERT INTO Macchina (VeicoloId, Targa, Cc, NumeroPorte)
                            VALUES (@VeicoloId, @Targa, @Cc, @NumeroPorte)";

                        using (var cmdMacchina = new SqlCommand(queryMacchina, connection, transaction))
                        {
                            cmdMacchina.Parameters.AddWithValue("@VeicoloId", veicoloId);
                            cmdMacchina.Parameters.AddWithValue("@Targa", macchina.Targa);
                            cmdMacchina.Parameters.AddWithValue("@Cc", macchina.Cc);
                            cmdMacchina.Parameters.AddWithValue("@NumeroPorte", macchina.NumeroPorte);

                            cmdMacchina.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(Macchina macchina)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string getVeicoloIdQuery = "SELECT VeicoloId FROM Macchina WHERE MacchinaId = @MacchinaId";
                        int veicoloId;
                        using (var cmdGet = new SqlCommand(getVeicoloIdQuery, connection, transaction))
                        {
                            cmdGet.Parameters.AddWithValue("@MacchinaId", macchina.MacchinaId);
                            veicoloId = Convert.ToInt32(cmdGet.ExecuteScalar());
                        }

                        string updateVeicolo = @"
                            UPDATE Veicolo SET NumeroRuote = @NumeroRuote, TipoAlimentazioneId = @TipoAlimentazioneId, 
                                   CategoriaId = @CategoriaId, Colore = @Colore, Marca = @Marca, 
                                   AnnoProduzione = @AnnoProduzione, Modello = @Modello
                            WHERE VeicoloId = @VeicoloId";

                        using (var cmdV = new SqlCommand(updateVeicolo, connection, transaction))
                        {
                            cmdV.Parameters.AddWithValue("@NumeroRuote", macchina.NumeroRuote);
                            cmdV.Parameters.AddWithValue("@TipoAlimentazioneId", macchina.TipoAlimentazioneId);
                            cmdV.Parameters.AddWithValue("@CategoriaId", macchina.CategoriaId);
                            cmdV.Parameters.AddWithValue("@Colore", (object)macchina.Colore ?? DBNull.Value);
                            cmdV.Parameters.AddWithValue("@Marca", macchina.Marca);
                            cmdV.Parameters.AddWithValue("@AnnoProduzione", macchina.AnnoProduzione);
                            cmdV.Parameters.AddWithValue("@Modello", macchina.Modello);
                            cmdV.Parameters.AddWithValue("@VeicoloId", veicoloId);
                            cmdV.ExecuteNonQuery();
                        }

                        string updateMacchina = @"
                            UPDATE Macchina SET Targa = @Targa, Cc = @Cc, NumeroPorte = @NumeroPorte
                            WHERE MacchinaId = @MacchinaId";

                        using (var cmdM = new SqlCommand(updateMacchina, connection, transaction))
                        {
                            cmdM.Parameters.AddWithValue("@Targa", macchina.Targa);
                            cmdM.Parameters.AddWithValue("@Cc", macchina.Cc);
                            cmdM.Parameters.AddWithValue("@NumeroPorte", macchina.NumeroPorte);
                            cmdM.Parameters.AddWithValue("@MacchinaId", macchina.MacchinaId);
                            cmdM.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Veicolo WHERE VeicoloId = (SELECT VeicoloId FROM Macchina WHERE MacchinaId = @Id)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private Macchina MapMacchina(SqlDataReader reader)
        {
            return new Macchina
            {
                MacchinaId = reader.GetInt32(reader.GetOrdinal("MacchinaId")),
                VeicoloId = reader.GetInt32(reader.GetOrdinal("VeicoloId")),
                Targa = reader.GetString(reader.GetOrdinal("Targa")),
                Cc = reader.GetInt32(reader.GetOrdinal("Cc")),
                NumeroPorte = reader.GetInt32(reader.GetOrdinal("NumeroPorte")),

                NumeroRuote = reader.GetInt32(reader.GetOrdinal("NumeroRuote")),
                TipoAlimentazioneId = reader.GetInt32(reader.GetOrdinal("TipoAlimentazioneId")),
                CategoriaId = reader.GetInt32(reader.GetOrdinal("CategoriaId")),
                Colore = reader.IsDBNull(reader.GetOrdinal("Colore")) ? null : reader.GetString(reader.GetOrdinal("Colore")),
                Marca = reader.GetString(reader.GetOrdinal("Marca")),
                AnnoProduzione = reader.GetInt32(reader.GetOrdinal("AnnoProduzione")),
                Modello = reader.GetString(reader.GetOrdinal("Modello"))
            };
        }
    }
}