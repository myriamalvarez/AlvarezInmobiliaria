using MySql.Data.MySqlClient;

namespace AlvarezInmobiliaria.Models
{
    public class RepositorioPago
    {
        protected readonly string connectionString;

        public RepositorioPago()
        {
            connectionString = "Server=localhost;User=root;Password=;Database=alvarez;SslMode=none";
        }

        public List<Pago> ObtenerPagos()
        {
            var res = new List<Pago>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                var query =
                    @"SELECT p.Id, NumeroPago, Fecha, Importe, ContratoId, c.InquilinoId, c.InmuebleId, inq.Nombre, inq.Apellido, i.Direccion
                          FROM Pago p JOIN Contrato c ON p.ContratoId = c.Id
                          JOIN Inquilino inq ON inq.Id = c.InquilinoId
                          JOIN Inmueble i ON i.Id = c.InmuebleId;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(
                                new Pago
                                {
                                    Id = reader.GetInt32("Id"),
                                    NumeroPago = reader.GetInt32("NumeroPago"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    Importe = reader.GetDecimal("Importe"),
                                    ContratoId = reader.GetInt32("ContratoId"),
                                    Contrato = new Contrato
                                    {
                                        Id = reader.GetInt32("ContratoId"),
                                        InquilinoId = reader.GetInt32("InquilinoId"),
                                        InmuebleId = reader.GetInt32("InmuebleId"),
                                        Inquilino = new Inquilino
                                        {
                                            Nombre = reader.GetString("Nombre"),
                                            Apellido = reader.GetString("Apellido"),
                                        },
                                        Inmueble = new Inmueble
                                        {
                                            Direccion = reader.GetString("Direccion"),
                                        },
                                    }
                                }
                            );
                        }
                    }
                    conn.Close();
                }
            }
            return res;
        }

        public int Alta(Pago pago)
        {
            var res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                var query =
                    @"INSERT INTO pago (NumeroPago, Fecha, Importe, ContratoId) 
                          SELECT COALESCE(MAX(NumeroPago), 0) + 1, @Fecha, @Importe, @ContratoId
                          FROM pago
                          WHERE ContratoId = @ContratoId
                          ON DUPLICATE KEY UPDATE NumeroPago = NumeroPago + 1;
                          SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NumeroPago", pago.NumeroPago);
                    cmd.Parameters.AddWithValue("@Fecha", pago.Fecha);
                    cmd.Parameters.AddWithValue("@Importe", pago.Importe);
                    cmd.Parameters.AddWithValue("@ContratoId", pago.ContratoId);
                    conn.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    pago.Id = res;
                    conn.Close();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                var query = @"DELETE FROM pago WHERE id = @Id;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    res = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }

        public int Modificacion(Pago pago)
        {
            int res = 0;
            using(MySqlConnection conn = new MySqlConnection(connectionString))
            {
                var query = @"UPDATE pago SET NumeroPago = @numeroPago, Fecha = @fecha, Importe = @importe, ContratoId = @contratoId
                            WHERE Id = @id;";
    
                using(MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                   
                   cmd.Parameters.AddWithValue("@numeroPago", pago.NumeroPago);
                   cmd.Parameters.AddWithValue("@fecha", pago.Fecha);
                   cmd.Parameters.AddWithValue("@importe", pago.Importe);
                   cmd.Parameters.AddWithValue("@contratoId", pago.ContratoId);
                   cmd.Parameters.AddWithValue("@id", pago.Id);
                   conn.Open();
                   res = cmd.ExecuteNonQuery();
                   conn.Close();
                }
            }
            return res;
        }

        public Pago ObtenerPorId(int id)
        {
            Pago res = null!;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                var query =
                    @"SELECT p.Id, NumeroPago, Fecha, Importe, ContratoId, c.InquilinoId, inq.Nombre, inq.Apellido, c.InmuebleId, inm.Direccion 
                          FROM pago p JOIN contrato c ON p.ContratoId = c.Id
                          JOIN inquilino inq ON c.InquilinoId = inq.Id
                          JOIN inmueble inm ON c.InmuebleId = inm.Id
                          WHERE p.Id=@id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res = new Pago
                            {
                                Id = reader.GetInt32("id"),
                                NumeroPago = reader.GetInt32("NumeroPago"),
                                Fecha = reader.GetDateTime("Fecha"),
                                Importe = reader.GetDecimal("Importe"),
                                ContratoId = reader.GetInt32("ContratoId"),
                                Contrato = new Contrato
                                {
                                    Id = reader.GetInt32("ContratoId"),
                                    InquilinoId = reader.GetInt32("InquilinoId"),
                                    InmuebleId = reader.GetInt32("InmuebleId"),
                                    Inquilino = new Inquilino
                                    {
                                        Id = reader.GetInt32("InquilinoId"),
                                        Nombre = reader.GetString("Nombre"),
                                        Apellido = reader.GetString("Apellido"),
                                    },
                                    Inmueble = new Inmueble
                                    {
                                        Id = reader.GetInt32("InmuebleId"),
                                        Direccion = reader.GetString("Direccion"),
                                    }
                                }
                            };
                        }
                    }
                    conn.Close();
                }
            }
            return res!;
        }
    }
}
