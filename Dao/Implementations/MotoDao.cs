using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using project_veicoli_ado.Dao.Interface;
using project_veicoli_ado.Models;

namespace project_veicoli_ado.Dao.Implementations
{
    public class MotoDao : IMotoDao
    {
        private readonly string _connectionString;

        public MotoDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Moto> GetAll()
        {
            var lista = new List<Moto>();
            string query = @"
                SELECT m.MotoId, m.VeicoloId, m.Targa, m.Cc,
                       v.NumeroRuote, v.TipoAlimentazioneId, v.CategoriaId, v.Colore, v.Marca, v.AnnoProduzione, v.Modello
                FROM Moto m
                INNER JOIN Veicolo v ON m.VeicoloId = v.VeicoloId";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MapMoto(reader));
                    }
                }
            }
            return lista;
        }

        public Moto GetById(int id)
        {
            Moto moto = null;
            string query = @"
                SELECT m.MotoId, m.VeicoloId, m.Targa, m.Cc,
                       v.NumeroRuote, v.TipoAlimentazioneId, v.CategoriaId, v.Colore, v.Marca, v.AnnoProduzione, v.Modello
                FROM Moto m
                INNER JOIN Veicolo v ON m.VeicoloId = v.VeicoloId
                WHERE m.MotoId = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        moto = MapMoto(reader);
                    }
                }
            }
            return moto;
        }

        public void Insert(Moto moto)
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
                            cmdVeicolo.Parameters.AddWithValue("@NumeroRuote", moto.NumeroRuote);
                            cmdVeicolo.Parameters.AddWithValue("@TipoAlimentazioneId", moto.TipoAlimentazioneId);
                            cmdVeicolo.Parameters.AddWithValue("@CategoriaId", moto.CategoriaId);
                            cmdVeicolo.Parameters.AddWithValue("@Colore", (object)moto.Colore ?? DBNull.Value);
                            cmdVeicolo.Parameters.AddWithValue("@Marca", moto.Marca);
                            cmdVeicolo.Parameters.AddWithValue("@AnnoProduzione", moto.AnnoProduzione);
                            cmdVeicolo.Parameters.AddWithValue("@Modello", moto.Modello);

                            veicoloId = Convert.ToInt32(cmdVeicolo.ExecuteScalar());
                        }

                        string queryMoto = @"
                            INSERT INTO Moto (VeicoloId, Targa, Cc)
                            VALUES (@VeicoloId, @Targa, @Cc)";

                        using (var cmdMoto = new SqlCommand(queryMoto, connection, transaction))
                        {
                            cmdMoto.Parameters.AddWithValue("@VeicoloId", veicoloId);
                            cmdMoto.Parameters.AddWithValue("@Targa", moto.Targa);
                            cmdMoto.Parameters.AddWithValue("@Cc", moto.Cc);

                            cmdMoto.ExecuteNonQuery();
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

        public void Update(Moto moto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string getVeicoloIdQuery = "SELECT VeicoloId FROM Moto WHERE MotoId = @MotoId";
                        int veicoloId;
                        using (var cmdGet = new SqlCommand(getVeicoloIdQuery, connection, transaction))
                        {
                            cmdGet.Parameters.AddWithValue("@MotoId", moto.MotoId);
                            veicoloId = Convert.ToInt32(cmdGet.ExecuteScalar());
                        }

                        string updateVeicolo = @"
                            UPDATE Veicolo SET NumeroRuote = @NumeroRuote, TipoAlimentazioneId = @TipoAlimentazioneId, 
                                   CategoriaId = @CategoriaId, Colore = @Colore, Marca = @Marca, 
                                   AnnoProduzione = @AnnoProduzione, Modello = @Modello
                            WHERE VeicoloId = @VeicoloId";

                        using (var cmdV = new SqlCommand(updateVeicolo, connection, transaction))
                        {
                            cmdV.Parameters.AddWithValue("@NumeroRuote", moto.NumeroRuote);
                            cmdV.Parameters.AddWithValue("@TipoAlimentazioneId", moto.TipoAlimentazioneId);
                            cmdV.Parameters.AddWithValue("@CategoriaId", moto.CategoriaId);
                            cmdV.Parameters.AddWithValue("@Colore", (object)moto.Colore ?? DBNull.Value);
                            cmdV.Parameters.AddWithValue("@Marca", moto.Marca);
                            cmdV.Parameters.AddWithValue("@AnnoProduzione", moto.AnnoProduzione);
                            cmdV.Parameters.AddWithValue("@Modello", moto.Modello);
                            cmdV.Parameters.AddWithValue("@VeicoloId", veicoloId);
                            cmdV.ExecuteNonQuery();
                        }

                        string updateMoto = @"
                            UPDATE Moto SET Targa = @Targa, Cc = @Cc
                            WHERE MotoId = @MotoId";

                        using (var cmdM = new SqlCommand(updateMoto, connection, transaction))
                        {
                            cmdM.Parameters.AddWithValue("@Targa", moto.Targa);
                            cmdM.Parameters.AddWithValue("@Cc", moto.Cc);
                            cmdM.Parameters.AddWithValue("@MotoId", moto.MotoId);
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
                string query = "DELETE FROM Veicolo WHERE VeicoloId = (SELECT VeicoloId FROM Moto WHERE MotoId = @Id)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private Moto MapMoto(SqlDataReader reader)
        {
            return new Moto
            {
                MotoId = reader.GetInt32(reader.GetOrdinal("MotoId")),
                VeicoloId = reader.GetInt32(reader.GetOrdinal("VeicoloId")),
                Targa = reader.GetString(reader.GetOrdinal("Targa")),
                Cc = reader.GetInt32(reader.GetOrdinal("Cc")),

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