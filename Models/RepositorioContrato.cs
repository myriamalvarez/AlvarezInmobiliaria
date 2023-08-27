using MySql.Data.MySqlClient;

namespace AlvarezInmobiliaria.Models;

public class RepositorioContrato 
{
    protected readonly string connectionString;
    public RepositorioContrato() 
    {
        connectionString = "Server=localhost;User=root;Password=;Database=alvarez;SslMode=none";
    }

    public int Alta(Contrato contrato)
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
        string query = @"INSERT INTO contrato (FechaInicio, FechaFin, Alquiler, InmuebleId, InquilinoId, Estado)
        VALUES (@fechaInicio, @fechaFin, @alquiler, @inmuebleId, @inquilinoId, @estado);
        SELECT LAST_INSERT_ID();";
        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
            command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
            command.Parameters.AddWithValue("@alquiler", contrato.Alquiler);
            command.Parameters.AddWithValue("@inmuebleId", contrato.InmuebleId);
            command.Parameters.AddWithValue("@inquilinoId", contrato.InquilinoId);
            command.Parameters.AddWithValue("@estado", contrato.Estado);
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
		int res = 0;
        int mta = 0;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
            var multa = @"SELECT Multa(@id);";
            using (var command = new MySqlCommand(multa, connection))
            {
                command.Parameters.AddWithValue("@id", id);
				connection.Open();
				mta = Convert.ToInt32(command.ExecuteScalar());
				connection.Close();
            }

			var query = @"DELETE FROM contrato WHERE Id = @id;";

			using (var command = new MySqlCommand(query, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return mta;
	}
    public int Modificacion(Contrato contrato) 
    {
        int res = 0;
        using(MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"UPDATE contrato SET fechaInicio = @fechaInicio, fechaFin = @fechaFin, alquiler = @alquiler, inmuebleId = @inmuebleId, inquilinoId = @inquilinoId, estado = @estado
                        WHERE Id = @id;";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@alquiler", contrato.Alquiler);
                command.Parameters.AddWithValue("@inmuebleId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@inquilinoId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@estado", contrato.Estado);
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
            var query = @"SELECT c.Id, FechaInicio, FechaFin, Alquiler, c.Estado, InmuebleId, InquilinoId, i.Direccion, inq.Nombre, inq.Apellido
                        FROM contrato c JOIN inmueble i ON (i.Id = c.InmuebleId)
                        JOIN inquilino inq ON (inq.Id = c.InquilinoId);";
            using(var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(new Contrato
                    {
                        Id = reader.GetInt32("Id"),
                        FechaInicio = DateOnly.FromDateTime(reader.GetDateTime("FechaInicio")),
                        FechaFin = DateOnly.FromDateTime(reader.GetDateTime("FechaFin")),
						Alquiler = reader.GetDecimal("Alquiler"),
						Estado = reader.GetString("Estado"),
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
						});
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
			var query = @"SELECT c.Id, FechaInicio, FechaFin, Alquiler, c.Estado, InmuebleId, InquilinoId, i.Direccion, inq.Nombre, inq.Apellido
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
                        FechaInicio = DateOnly.FromDateTime(reader.GetDateTime("FechaInicio")),
                        FechaFin = DateOnly.FromDateTime(reader.GetDateTime("FechaFin")),
						Alquiler = reader.GetDecimal("Alquiler"),
						Estado = reader.GetString("Estado"),
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
        }
    
