using MySql.Data.MySqlClient;

namespace AlvarezInmobiliaria.Models;

public class RepositorioInquilino
{
    protected readonly string connectionString;

    public RepositorioInquilino()
    {
        connectionString = "Server=localhost;User=root;Password=;Database=alvarez;SslMode=none";
    }

    public List<Inquilino> ObtenerInquilinos()
    {
        var res = new List<Inquilino>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query = "SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM inquilino;";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                   {
                    while (reader.Read())
                    {
                        res.Add(new Inquilino
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Dni = reader.GetString("Dni"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                        });
                    }
                   } 
                   conn.Close();
            } 
        }
            return res;
    }
    

    public int Alta(Inquilino inquilino) 
    {
         var res = -1;
         using(MySqlConnection conn = new MySqlConnection(connectionString))
         {
            var query = @"INSERT INTO inquilino (Nombre, Apellido, Dni, Telefono, Email)
                          VALUES(@Nombre, @Apellido, @Dni, @Telefono, @Email);
                          SELECT LAST_INSERT_ID()";
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                cmd.Parameters.AddWithValue("@Dni", inquilino.Dni);
                cmd.Parameters.AddWithValue("@Telefono", inquilino.Telefono);
                cmd.Parameters.AddWithValue("@Email", inquilino.Email);
                conn.Open();
                res = Convert.ToInt32(cmd.ExecuteScalar());
                inquilino.Id = res;
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
            var query = @"DELETE FROM inquilino WHERE id = @Id;";
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

    public int Modificacion(Inquilino inquilino)
    {
        int res = -1;
        using(MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query = @"UPDATE inquilino SET Nombre = @nombre, Apellido = @apellido, Dni = @dni, Telefono = @telefono, Email = @email
                        WHERE Id = @id;";

            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
               
               cmd.Parameters.AddWithValue("@nombre", inquilino.Nombre);
               cmd.Parameters.AddWithValue("apellido", inquilino.Apellido);
               cmd.Parameters.AddWithValue("@dni", inquilino.Dni);
               cmd.Parameters.AddWithValue("@telefono", inquilino.Telefono);
               cmd.Parameters.AddWithValue("@email", inquilino.Email);
               cmd.Parameters.AddWithValue("@id", inquilino.Id);
               conn.Open();
               res = cmd.ExecuteNonQuery();
               conn.Close(); 
            }
        }
        return res;
    }

    public Inquilino ObtenerPorId(int id)
    {
        Inquilino res = null!;
        using(MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var query = "SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM inquilino WHERE Id=@id";
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                conn.Open();
                using(MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Inquilino
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