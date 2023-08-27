using System.Reflection.Metadata.Ecma335;
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
           var query = @"SELECT i.Id, Direccion, Uso, Tipo, Ambientes, Latitud, Longitud, Estado, Precio, i.PropietarioId, p.Nombre, p.Apellido 
                        FROM inmueble i
                        INNER JOIN propietario p ON p.Id = i.PropietarioId;"; 

           using (MySqlCommand cmd = new MySqlCommand(query, conn))
           {
             conn.Open();
             using (MySqlDataReader reader = cmd.ExecuteReader())
             {
                while (reader.Read())
                {
                    res.Add(new Inmueble
                    {
                        Id = reader.GetInt32("Id"),
                        Direccion = reader.GetString("Direccion"),
                        Uso = reader.GetInt32("Uso"),
                        Tipo = reader.GetInt32("Tipo"),
                        Ambientes = reader.GetInt32("Ambientes"),
                        Latitud = reader.GetDecimal("Latitud"),
                        Longitud = reader.GetDecimal("Longitud"),
                        Estado = reader.GetInt32("Estado"),
                        Precio = reader.GetDecimal("Precio"),
                        PropietarioId = reader.GetInt32("PropietarioId"),
                        Propietario = new Propietario
                        {
                            Id = reader.GetInt32("PropietarioId"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                        }
                    });
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
        using(MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query = @"INSERT INTO inmueble (Direccion, Uso, Tipo, Estado, Ambientes, Latitud, Longitud, Precio, PropietarioId) 
                          VALUES(@direccion, @uso, @tipo, @estado, @ambientes, @latitud, @longitud, @precio, @propietarioId);
                          SELECT LAST_INSERT_ID()";

                          using(MySqlCommand cmd = new MySqlCommand(query, conn))
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
            var query = @"UPDATE inmueble SET Direccion = @Direccion, Uso = @uso, Tipo = @tipo, Ambientes = @ambientes, Latitud = @latitud,
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
                res =cmd.ExecuteNonQuery();
                conn.Close();
            }               
        }
        return res;
    }

    public Inmueble ObtenerPorId(int id)
    {
        Inmueble res = null!;
        using(MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query = @"SELECT i.Id, Direccion, Uso, Tipo, Ambientes, Latitud, Longitud, Estado, Precio, PropietarioId, p.Nombre, p.Apellido 
                          FROM inmueble i INNER JOIN propietario p ON p.Id = i.PropietarioId
                          WHERE i.Id = @Id;";
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("Id", id);
                conn.Open();
                using(MySqlDataReader reader = cmd.ExecuteReader())
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
                            Estado = reader.GetInt32("estado"),
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
            var query = @"SELECT Id, Direccion, Uso, Tipo, Ambientes, Precio
                        FROM inmueble WHERE Estado = 1;";

            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(new Inmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = reader.GetInt32("Uso"),
                            Tipo = reader.GetInt32("Tipo"),
                            Ambientes = reader.GetInt32("Ambientes"),
                            Precio = reader.GetDecimal("Precio"),
                        });
                    }
                }
                conn.Close();
            }
        }
        return res;
    }

}