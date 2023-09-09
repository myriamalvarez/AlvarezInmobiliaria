using System.Data;
using MySql.Data.MySqlClient;

namespace AlvarezInmobiliaria.Models
{
    public class RepositorioUsuario
    {
        protected readonly string connectionString;

        public RepositorioUsuario()
        {
            connectionString = "Server=localhost;User=root;Password=;Database=alvarez;SslMode=none";
        }

        public int Alta(Usuario usuario)
        {
            string avatarDefault = "/img/avatarDefault.png";
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query =
                    @"INSERT INTO usuario (Nombre, Apellido, Avatar, Email, Clave, Rol)
                                 VALUES (@nombre, @apellido, @avatar, @email, @clave, @rol);
                                 SELECT LAST_INSERT_ID();";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    if (String.IsNullOrEmpty(usuario.Avatar))
                    {
                        cmd.Parameters.AddWithValue("@avatar", avatarDefault);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@avatar", usuario.Avatar);
                    }
                    cmd.Parameters.AddWithValue("@email", usuario.Email);
                    cmd.Parameters.AddWithValue("@clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("@rol", usuario.Rol);
                    conn.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    usuario.Id = res;
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
                var query = "DELETE FROM usuario WHERE id = @Id";
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

        public int Modificacion(Usuario usuario)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                var query =
                    @"UPDATE usuario SET Nombre = @nombre, Apellido = @Apellido, Avatar = @avatar, Email = @email, Clave = @clave, Rol = @rol
                             WHERE Id = @id;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@avatar", usuario.Avatar);
                    cmd.Parameters.AddWithValue("@email", usuario.Email);
                    cmd.Parameters.AddWithValue("@clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("@rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@id", usuario.Id);
                    conn.Open();
                    res = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }

        

        public List<Usuario> ObtenerUsuarios()
        {
            var res = new List<Usuario>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                var query = "SELECT Id, Nombre, Apellido, Avatar, Email, Rol FROM usuario;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(
                                new Usuario
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),
                                    Avatar = reader.GetString("Avatar"),
                                    Email = reader.GetString("Email"),
                                    Rol = reader.GetInt32("Rol"),
                                }
                            );
                        }
                    }
                    conn.Close();
                }
            }
            return res;
        }

        public Usuario ObtenerPorId(int id)
        {
            Usuario res = null!;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                var query =
                    @"SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol 
                             FROM usuario WHERE Id = @id;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res = new Usuario
                            {
                                Id = reader.GetInt32("Id"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Avatar = reader.GetString("Avatar"),
                                Email = reader.GetString("Email"),
                                Clave = reader.GetString("Clave"),
                                Rol = reader.GetInt32("Rol"),
                            };
                        }
                    }
                    conn.Close();
                }
            }
            return res!;
        }

        public Usuario ObtenerPorMail(string email)
        {
            Usuario res = null!;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query =
                    @"SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol
                                 FROM usuario WHERE Email=@email;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res = new Usuario
                            {
                                Id = reader.GetInt32("Id"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Avatar = reader.GetString("Avatar"),
                                Email = reader.GetString("Email"),
                                Clave = reader.GetString("Clave"),
                                Rol = reader.GetInt32("Rol"),
                            };
                        }
                    }
                    conn.Close();
                }
            }
            return res!;
        }
        public int CambiarClave(int id, string ClaveNueva)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string querry = "UPDATE usuario SET Clave=@clave WHERE Id=@id";
                using (MySqlCommand cmd = new MySqlCommand(querry, conn))
                {
                    cmd.Parameters.AddWithValue("@clave", ClaveNueva);
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    res = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int EditarDatos(int id, string nombre, string apellido, string email, int rol)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string querry = "UPDATE usuario SET nombre=@nombre, apellido=@apellido, email=@email, rol=@rol WHERE id=@id";
                using (MySqlCommand cmd = new MySqlCommand(querry, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellido", apellido);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@rol", rol);
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    res = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
    }
}
