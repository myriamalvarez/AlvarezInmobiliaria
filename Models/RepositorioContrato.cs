using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace AlvarezInmobiliaria.Models;

public class RepositorioContrato
{
    protected readonly string connectionString;

    public RepositorioContrato()
    {
        connectionString = "Server=localhost;User=root;Password=;Database=malvarez;SslMode=none";
    }

    public int Alta(Contrato contrato)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query =
                @"INSERT INTO contrato (FechaInicio, FechaFin, Alquiler, InmuebleId, InquilinoId)
        VALUES (@fechaInicio, @fechaFin, @alquiler, @inmuebleId, @inquilinoId);
        SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@alquiler", contrato.Alquiler);
                command.Parameters.AddWithValue("@inmuebleId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@inquilinoId", contrato.InquilinoId);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                contrato.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    public int Baja(int id)
    {
        int res = -1;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query = @"DELETE FROM contrato WHERE Id = @id;";
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

    public int Modificacion(Contrato contrato)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query =
                @"UPDATE contrato SET FechaInicio = @fechaInicio, FechaFin = @fechaFin, Alquiler = @alquiler, 
                                InmuebleId = @inmuebleId, InquilinoId = @inquilinoId
                        WHERE Id = @id;";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@alquiler", contrato.Alquiler);
                command.Parameters.AddWithValue("@inmuebleId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@inquilinoId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@id", contrato.Id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public List<Contrato> ObtenerContratos()
    {
        List<Contrato> contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query =
                @"SELECT c.Id, FechaInicio, FechaFin, Alquiler, InmuebleId, InquilinoId, i.Direccion, inq.Nombre, inq.Apellido
                        FROM contrato c JOIN inmueble i ON (i.Id = c.InmuebleId)
                        JOIN inquilino inq ON (inq.Id = c.InquilinoId);";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(
                        new Contrato
                        {
                            Id = reader.GetInt32("Id"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Alquiler = reader.GetDecimal("Alquiler"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            Inmueble = new Inmueble { Direccion = reader.GetString("Direccion"), },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                            }
                        }
                    );
                }
                connection.Close();
            }
        }
        return contratos;
    }

    public Contrato ObtenerPorId(int id)
    {
        Contrato? contrato = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query =
                @"SELECT c.Id, FechaInicio, FechaFin, Alquiler, InmuebleId, InquilinoId, i.Direccion, inq.Nombre, inq.Apellido
			FROM contrato c
			INNER JOIN inmueble i ON i.Id = c.InmuebleId
            INNER JOIN inquilino inq ON inq.Id = c.InquilinoId
			WHERE c.Id = @Id;";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            Id = reader.GetInt32("Id"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Alquiler = reader.GetDecimal("Alquiler"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            Inmueble = new Inmueble 
                            { 
                                Direccion = reader.GetString("Direccion"), 
                            },
                                Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                            }
                        };
                    }
                }
            }
            connection.Close();
        }
        return contrato!;
    }
 
    public List<Contrato> ContratosVigentes(DateTime fecha)
    {
        List<Contrato> contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query =
                @"SELECT c.Id, FechaInicio, FechaFin, Alquiler, InmuebleId, InquilinoId, i.Direccion, inq.Nombre, inq.Apellido
			FROM contrato c
			INNER JOIN inmueble i ON i.Id = c.InmuebleId
            INNER JOIN inquilino inq ON inq.Id = c.InquilinoId
			WHERE @fecha BETWEEN FechaInicio AND FechaFin;";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@fecha", MySqlDbType.Date).Value = fecha.Date;
                command.CommandType = CommandType.Text;
                connection.Open();
                using (var reader = command.ExecuteReader())
                
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32("Id"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Alquiler = reader.GetDecimal("Alquiler"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            Inmueble = new Inmueble 
                            { 
                                Direccion = reader.GetString("Direccion"), 
                            },
                                Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                            }
                        };
                        contratos.Add(contrato);
                    }
                    
            }
            connection.Close();
        }
        return contratos!;
        
    }
    public List<Contrato> BuscarPorInmuble(int id)
    {
      List<Contrato> contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query =
                @"SELECT c.Id, FechaInicio, FechaFin, Alquiler, InmuebleId, InquilinoId, i.Direccion, inq.Nombre, inq.Apellido
                        FROM contrato c JOIN inmueble i ON i.Id = c.InmuebleId
                        JOIN inquilino inq ON inq.Id = c.InquilinoId
                        WHERE InmuebleId = @id;";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("id", MySqlDbType.Int32).Value = id;
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(
                        new Contrato
                        {
                            Id = reader.GetInt32("Id"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Alquiler = reader.GetDecimal("Alquiler"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            Inmueble = new Inmueble { Direccion = reader.GetString("Direccion"), },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                            }
                        }
                    );
                }
                connection.Close();
            }
        }
        return contratos;  
    }
    public int CancelarContrato(Contrato contrato)
    {
        int res =-1;
        using(MySqlConnection conn = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE contrato SET FechaFin=@fechaFin WHERE Id = @id";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@fechaFin", DateTime.Now.AddDays(-1));
                cmd.Parameters.AddWithValue("@id", contrato.Id);
                conn.Open();
                res = cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }
}
