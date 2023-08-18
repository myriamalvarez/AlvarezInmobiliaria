using MySql.Data.MySqlClient;

namespace AlvarezInmobiliaria.Models;

public class RepositorioPropietario
{
    protected readonly string connectionString;

    public RepositorioPropietario()
    {
        connectionString = "Server=localhost;User=root;Password=;Database=alvarez;SslMode=none";
    }

    public List<Propietario> ObtenerPropietarios()
    {
        var res = new List<Propietario>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query = "SELECT Id, Nombre, Apellido, Telefono FROM propietario;";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                   {
                    while (reader.Read())
                    {
                        res.Add(new Propietario
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Telefono = reader.GetString("Telefono"),
                        });
                    }
                   } 
                   conn.Close();
            } 
        }
            return res;
    }
    

    public int Alta(Propietario propietario)
    {
         var res = -1;
         using(MySqlConnection conn = new MySqlConnection(connectionString))
         {
            var query = @"INSERT INTO propietario (Nombre, Apellido, Dni, Telefono, Email)
                          VALUES(@Nombre, @Apellido, @Dni, @Telefono, @Email);
                          SELECT LAST_INSERT_ID()";
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                cmd.Parameters.AddWithValue("@Dni", propietario.Dni);
                cmd.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                cmd.Parameters.AddWithValue("@Email", propietario.Email);
                conn.Open();
                res = Convert.ToInt32(cmd.ExecuteScalar());
                propietario.Id = res;
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
            var query = @"DELETE FROM propietario WHERE id = @Id;";
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

    public int Modificacion(Propietario propietario)
    {
        int res = 0;
        using(MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query = @"UPDATE propietario SET Nombre = @nombre, Apellido = @apellido, Dni = @dni, Telefono = @telefono, Email = @email
                        WHERE Id = @id;";

            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
               
               cmd.Parameters.AddWithValue("@nombre", propietario.Nombre);
               cmd.Parameters.AddWithValue("@apellido", propietario.Apellido);
               cmd.Parameters.AddWithValue("@dni", propietario.Dni);
               cmd.Parameters.AddWithValue("@telefono", propietario.Telefono);
               cmd.Parameters.AddWithValue("@email", propietario.Email);
               cmd.Parameters.AddWithValue("@id", propietario.Id);
               conn.Open();
               res = cmd.ExecuteNonQuery();
               conn.Close(); 
            }
        }
        return res;
    }

    public Propietario ObtenerPorId(int id)
    {
        Propietario res = null!;
        using(MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query = "SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM propietario WHERE Id=@id";
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                conn.Open();
                using(MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Propietario
                        {
                            Id = reader.GetInt32("id"),
                            Nombre = reader.GetString("nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Dni = reader.GetString("Dni"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                        };
                    }
                }
                conn.Close();
            }
        } 
        return res!;
    }
}