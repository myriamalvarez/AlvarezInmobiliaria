using System.Data;
using MySql.Data.MySqlClient;

namespace AlvarezInmobiliaria.Models;

public class RepositorioInmueble
{
    protected readonly string connectionString;

    public RepositorioInmueble()
    {
        connectionString = "Server=localhost;User=root;Password=;Database=alvarez;SslMode=none";
    }

    public List<Inmueble> ObtenerInmuebles()
    {
        var res = new List<Inmueble>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query =
                @"SELECT i.Id, Direccion, Uso, Tipo, Ambientes, Latitud, Longitud, Estado, Precio, i.PropietarioId, p.Nombre, p.Apellido 
                        FROM inmueble i
                        INNER JOIN propietario p ON p.Id = i.PropietarioId;";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(
                            new Inmueble
                            {
                                Id = reader.GetInt32("Id"),
                                Direccion = reader.GetString("Direccion"),
                                Uso = reader.GetInt32("Uso"),
                                Tipo = reader.GetInt32("Tipo"),
                                Ambientes = reader.GetInt32("Ambientes"),
                                Latitud = reader.GetDecimal("Latitud"),
                                Longitud = reader.GetDecimal("Longitud"),
                                Estado = reader.GetBoolean("Estado"),
                                Precio = reader.GetDecimal("Precio"),
                                PropietarioId = reader.GetInt32("PropietarioId"),
                                Propietario = new Propietario
                                {
                                    Id = reader.GetInt32("PropietarioId"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),
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

    public int Alta(Inmueble inmueble)
    {
        var res = -1;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query =
                @"INSERT INTO inmueble (Direccion, Uso, Tipo, Estado, Ambientes, Latitud, Longitud, Precio, PropietarioId) 
                          VALUES(@direccion, @uso, @tipo, @estado, @ambientes, @latitud, @longitud, @precio, @propietarioId);
                          SELECT LAST_INSERT_ID()";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                cmd.Parameters.AddWithValue("@uso", inmueble.Uso);
                cmd.Parameters.AddWithValue("@tipo", inmueble.Tipo);
                cmd.Parameters.AddWithValue("@estado", inmueble.Estado);
                cmd.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
                cmd.Parameters.AddWithValue("@latitud", inmueble.Latitud);
                cmd.Parameters.AddWithValue("@longitud", inmueble.Longitud);
                cmd.Parameters.AddWithValue("precio", inmueble.Precio);
                cmd.Parameters.AddWithValue("propietarioId", inmueble.PropietarioId);
                conn.Open();
                res = Convert.ToInt32(cmd.ExecuteScalar());
                inmueble.Id = res;
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
            var query = @"DELETE FROM inmueble WHERE id = @Id;";
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

    public int Modificacion(Inmueble inmueble)
    {
        int res = -1;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query =
                @"UPDATE inmueble SET Direccion = @Direccion, Uso = @uso, Tipo = @tipo, Ambientes = @ambientes, Latitud = @latitud,
                          Longitud = @longitud, Estado = @estado, Precio = @precio, PropietarioId = @propietarioId
                          WHERE Id = @id;";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                cmd.Parameters.AddWithValue("@uso", inmueble.Uso);
                cmd.Parameters.AddWithValue("@tipo", inmueble.Tipo);
                cmd.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
                cmd.Parameters.AddWithValue("@latitud", inmueble.Latitud);
                cmd.Parameters.AddWithValue("@longitud", inmueble.Longitud);
                cmd.Parameters.AddWithValue("@estado", inmueble.Estado);
                cmd.Parameters.AddWithValue("@precio", inmueble.Precio);
                cmd.Parameters.AddWithValue("@propietarioId", inmueble.PropietarioId);
                cmd.Parameters.AddWithValue("@id", inmueble.Id);
                conn.Open();
                res = cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }

    public Inmueble ObtenerPorId(int id)
    {
        Inmueble res = null!;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query =
                @"SELECT i.Id, Direccion, Uso, Tipo, Ambientes, Latitud, Longitud, Estado, Precio, PropietarioId, p.Nombre, p.Apellido 
                          FROM inmueble i INNER JOIN propietario p ON p.Id = i.PropietarioId
                          WHERE i.Id = @Id;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("Id", id);
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Inmueble
                        {
                            Id = reader.GetInt32("id"),
                            Direccion = reader.GetString("direccion"),
                            Uso = reader.GetInt32("uso"),
                            Tipo = reader.GetInt32("tipo"),
                            Ambientes = reader.GetInt32("ambientes"),
                            Latitud = reader.GetDecimal("latitud"),
                            Longitud = reader.GetDecimal("longitud"),
                            Estado = reader.GetBoolean("estado"),
                            Precio = reader.GetDecimal("precio"),
                            PropietarioId = reader.GetInt32("propietarioId"),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32("propietarioId"),
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido"),
                            }
                        };
                    }
                }
            }
            conn.Close();
        }
        return res;
    }

    public List<Inmueble> ObtenerInmueblesDisponibles()
    {
        var res = new List<Inmueble>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query =
                @"SELECT i.Id, Direccion, Uso, Tipo, Ambientes, Precio, i.PropietarioId, p.Nombre, p.Apellido
                        FROM inmueble i INNER JOIN propietario p ON p.Id = i.PropietarioId WHERE Estado = 1;";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(
                            new Inmueble
                            {
                                Id = reader.GetInt32("Id"),
                                Direccion = reader.GetString("Direccion"),
                                Uso = reader.GetInt32("Uso"),
                                Tipo = reader.GetInt32("Tipo"),
                                Ambientes = reader.GetInt32("Ambientes"),
                                Precio = reader.GetDecimal("Precio"),
                                PropietarioId = reader.GetInt32("propietarioId"),
                                Propietario = new Propietario
                                {
                                    Id = reader.GetInt32("propietarioId"),
                                    Nombre = reader.GetString("nombre"),
                                    Apellido = reader.GetString("apellido"),
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
    public List<Inmueble> BuscarPorFecha(DateTime desde, DateTime hasta)
    {
       var res = new List<Inmueble>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query =
                @"SELECT i.Id, Direccion, Uso, Tipo, Ambientes, Latitud, Longitud, Precio, PropietarioId, p.Nombre, p.Apellido
                        FROM inmueble i JOIN Propietario p ON i.PropietarioId = p.Id
                        WHERE i.Id IN (SELECT InmuebleId FROM contrato c 
                        WHERE i.Estado = 1
                        AND ((FechaInicio < @desde)AND (FechaFin < @desde))
                        OR ((FechaInicio > @hasta)AND (FechaFin > @hasta))
                        AND ((FechaInicio < @desde)AND (FechaFin > @hasta))
                        OR ((FechaInicio > @desde)AND (FechaFin < @hasta))
                        AND(FechaInicio NOT BETWEEN @desde AND @hasta)
                        AND(FechaFin NOT BETWEEN @desde AND @hasta))
                        OR i.Id NOT IN (SELECT InmuebleId FROM contrato);";


            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.Add("@desde", MySqlDbType.Date).Value = desde.Date;
                cmd.Parameters.Add("@hasta", MySqlDbType.Date).Value = hasta.Date;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", 0);
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(
                            new Inmueble
                            {
                                Id = reader.GetInt32("Id"),
                                Direccion = reader.GetString("Direccion"),
                                Uso = reader.GetInt32("Uso"),
                                Tipo = reader.GetInt32("Tipo"),
                                Ambientes = reader.GetInt32("Ambientes"),
                                Latitud = reader.GetDecimal("Latitud"),
                                Longitud = reader.GetDecimal("Longitud"),
                                Precio = reader.GetDecimal("Precio"),
                                PropietarioId = reader.GetInt32("propietarioId"),
                                Propietario = new Propietario
                                {
                                    Id = reader.GetInt32("propietarioId"),
                                    Nombre = reader.GetString("nombre"),
                                    Apellido = reader.GetString("apellido"),
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
    public List<Inmueble> BuscarPorPropietario(int id)
    {
        List<Inmueble> res = new List<Inmueble>();
        Inmueble e = null!;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query =
                @"SELECT i.Id, Direccion, Uso, Tipo, Ambientes, Latitud, Longitud, Estado, Precio, PropietarioId, p.Nombre, p.Apellido 
                          FROM inmueble i INNER JOIN propietario p ON p.Id = i.PropietarioId
                          WHERE PropietarioId = @id;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                cmd.CommandType = CommandType.Text;
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        e = new Inmueble
                        {
                            Id = reader.GetInt32("id"),
                            Direccion = reader.GetString("direccion"),
                            Uso = reader.GetInt32("uso"),
                            Tipo = reader.GetInt32("tipo"),
                            Ambientes = reader.GetInt32("ambientes"),
                            Latitud = reader.GetDecimal("latitud"),
                            Longitud = reader.GetDecimal("longitud"),
                            Estado = reader.GetBoolean("estado"),
                            Precio = reader.GetDecimal("precio"),
                            PropietarioId = reader.GetInt32("propietarioId"),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32("propietarioId"),
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido"),
                            }
                        };
                        res.Add(e);
                    }
                    
                }
                conn.Close();
            }
          return res;  
        }
        
    }

}
