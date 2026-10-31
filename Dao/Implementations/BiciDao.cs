using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using project_veicoli_ado.Dao.Interface;
using project_veicoli_ado.Models;

namespace project_veicoli_ado.Dao.Implementations
{
    public class BiciDao : IBiciDao
    {
        private readonly string _connectionString;

        public BiciDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Bici> GetAll()
        {
            var lista = new List<Bici>();
            string query = @"
                SELECT b.BiciId, b.VeicoloId, b.NumeroCorone, b.NumeroMarce, b.TipoFrenoId, b.TipoSospensioneId, b.Pieghevole,
                       v.NumeroRuote, v.TipoAlimentazioneId, v.CategoriaId, v.Colore, v.Marca, v.AnnoProduzione, v.Modello
                FROM Bici b
                INNER JOIN Veicolo v ON b.VeicoloId = v.VeicoloId";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MapBici(reader));
                    }
                }
            }
            return lista;
        }

        public Bici GetById(int id)
        {
            Bici bici = null;
            string query = @"
                SELECT b.BiciId, b.VeicoloId, b.NumeroCorone, b.NumeroMarce, b.TipoFrenoId, b.TipoSospensioneId, b.Pieghevole,
                       v.NumeroRuote, v.TipoAlimentazioneId, v.CategoriaId, v.Colore, v.Marca, v.AnnoProduzione, v.Modello
                FROM Bici b
                INNER JOIN Veicolo v ON b.VeicoloId = v.VeicoloId
                WHERE b.BiciId = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bici = MapBici(reader);
                    }
                }
            }
            return bici;
        }

        public void Insert(Bici bici)
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
                            cmdVeicolo.Parameters.AddWithValue("@NumeroRuote", bici.NumeroRuote);
                            cmdVeicolo.Parameters.AddWithValue("@TipoAlimentazioneId", bici.TipoAlimentazioneId);
                            cmdVeicolo.Parameters.AddWithValue("@CategoriaId", bici.CategoriaId);
                            cmdVeicolo.Parameters.AddWithValue("@Colore", (object)bici.Colore ?? DBNull.Value);
                            cmdVeicolo.Parameters.AddWithValue("@Colore", (object)bici.Colore ?? DBNull.Value);
                            cmdVeicolo.Parameters.AddWithValue("@Marca", bici.Marca);
                            cmdVeicolo.Parameters.AddWithValue("@AnnoProduzione", bici.AnnoProduzione);
                            cmdVeicolo.Parameters.AddWithValue("@Modello", bici.Modello);

                            veicoloId = Convert.ToInt32(cmdVeicolo.ExecuteScalar());
                        }

                        string queryBici = @"
                            INSERT INTO Bici (VeicoloId, NumeroCorone, NumeroMarce, TipoFrenoId, TipoSospensioneId, Pieghevole)
                            VALUES (@VeicoloId, @NumeroCorone, @NumeroMarce, @TipoFrenoId, @TipoSospensioneId, @Pieghevole)";

                        using (var cmdBici = new SqlCommand(queryBici, connection, transaction))
                        {
                            cmdBici.Parameters.AddWithValue("@VeicoloId", veicoloId);
                            cmdBici.Parameters.AddWithValue("@NumeroCorone", bici.NumeroCorone);
                            cmdBici.Parameters.AddWithValue("@NumeroMarce", bici.NumeroMarce);
                            cmdBici.Parameters.AddWithValue("@TipoFrenoId", bici.TipoFrenoId);
                            cmdBici.Parameters.AddWithValue("@TipoSospensioneId", bici.TipoSospensioneId);
                            cmdBici.Parameters.AddWithValue("@Pieghevole", bici.Pieghevole);

                            cmdBici.ExecuteNonQuery();
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

        public void Update(Bici bici)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string getVeicoloIdQuery = "SELECT VeicoloId FROM Bici WHERE BiciId = @BiciId";
                        int veicoloId;
                        using (var cmdGet = new SqlCommand(getVeicoloIdQuery, connection, transaction))
                        {
                            cmdGet.Parameters.AddWithValue("@BiciId", bici.BiciId);
                            veicoloId = Convert.ToInt32(cmdGet.ExecuteScalar());
                        }

                        string updateVeicolo = @"
                            UPDATE Veicolo SET NumeroRuote = @NumeroRuote, TipoAlimentazioneId = @TipoAlimentazioneId, 
                                   CategoriaId = @CategoriaId, Colore = @Colore, Marca = @Marca, 
                                   AnnoProduzione = @AnnoProduzione, Modello = @Modello
                            WHERE VeicoloId = @VeicoloId";

                        using (var cmdV = new SqlCommand(updateVeicolo, connection, transaction))
                        {
                            cmdV.Parameters.AddWithValue("@NumeroRuote", bici.NumeroRuote);
                            cmdV.Parameters.AddWithValue("@TipoAlimentazioneId", bici.TipoAlimentazioneId);
                            cmdV.Parameters.AddWithValue("@CategoriaId", bici.CategoriaId);
                            cmdV.Parameters.AddWithValue("@Colore", (object)bici.Colore ?? DBNull.Value);
                            cmdV.Parameters.AddWithValue("@Marca", bici.Marca);
                            cmdV.Parameters.AddWithValue("@AnnoProduzione", bici.AnnoProduzione);
                            cmdV.Parameters.AddWithValue("@Modello", bici.Modello);
                            cmdV.Parameters.AddWithValue("@VeicoloId", veicoloId);
                            cmdV.ExecuteNonQuery();
                        }

                        string updateBici = @"
                            UPDATE Bici SET NumeroCorone = @NumeroCorone, NumeroMarce = @NumeroMarce, 
                                   TipoFrenoId = @TipoFrenoId, TipoSospensioneId = @TipoSospensioneId, Pieghevole = @Pieghevole
                            WHERE BiciId = @BiciId";

                        using (var cmdB = new SqlCommand(updateBici, connection, transaction))
                        {
                            cmdB.Parameters.AddWithValue("@NumeroCorone", bici.NumeroCorone);
                            cmdB.Parameters.AddWithValue("@NumeroMarce", bici.NumeroMarce);
                            cmdB.Parameters.AddWithValue("@TipoFrenoId", bici.TipoFrenoId);
                            cmdB.Parameters.AddWithValue("@TipoSospensioneId", bici.TipoSospensioneId);
                            cmdB.Parameters.AddWithValue("@Pieghevole", bici.Pieghevole);
                            cmdB.Parameters.AddWithValue("@BiciId", bici.BiciId);
                            cmdB.ExecuteNonQuery();
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
                string query = "DELETE FROM Veicolo WHERE VeicoloId = (SELECT VeicoloId FROM Bici WHERE BiciId = @Id)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private Bici MapBici(SqlDataReader reader)
        {
            return new Bici
            {
                BiciId = reader.GetInt32(reader.GetOrdinal("BiciId")),
                VeicoloId = reader.GetInt32(reader.GetOrdinal("VeicoloId")),
                NumeroCorone = reader.GetInt32(reader.GetOrdinal("NumeroCorone")),
                NumeroMarce = reader.GetInt32(reader.GetOrdinal("NumeroMarce")),
                TipoFrenoId = reader.GetInt32(reader.GetOrdinal("TipoFrenoId")),
                TipoSospensioneId = reader.GetInt32(reader.GetOrdinal("TipoSospensioneId")),
                Pieghevole = reader.GetBoolean(reader.GetOrdinal("Pieghevole")),

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