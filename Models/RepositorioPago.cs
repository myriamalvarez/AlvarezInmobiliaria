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

       
    }
}

