using System.ComponentModel.DataAnnotations;

namespace AlvarezInmobiliaria.Models;

public enum enUsos
{
    Resindencial = 1,
    Comercial = 2,
}

public enum enTipos
{
    Casa = 1,
    Departamento = 2,
    Local = 3,
    Oficina = 4,
    Deposito = 5,
}

public class Inmueble
{
    [Display(Name = "Código")]
    public int Id { get; set; }

    [Display(Name = "Dirección")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public string Direccion { get; set; } = "";

    public int Uso { get; set; } = 0;  

    public int Tipo { get; set; } = 0;

    public int Ambientes { get; set; } = 0;

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; } 

    public bool Estado { get; set; } = true;

    public decimal Precio { get; set;}

    [Display(Name = "Propietario")]
    public int PropietarioId { get; set;}
    public Propietario? Propietario { get; set;}

    public string? EstadoNombre => Estado == true ? "Disponible" : "No Disponible";

    public string UsoNombre => Uso > 0 ? ((enUsos)Uso).ToString() : "";

    public static IDictionary<int, string> ObtenerUsos()
    {
        SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
        Type tipoEnumUso = typeof(enUsos);
        foreach (var valor in Enum.GetValues(tipoEnumUso))
        {
            usos.Add((int)valor, Enum.GetName(tipoEnumUso, valor)!);
        }
        return usos;
    }

    public string TipoNombre => Tipo > 0 ? ((enTipos)Tipo).ToString() : "";

    public static IDictionary<int, string> ObtenerTipos()
    {
        SortedDictionary<int, string> tipos = new SortedDictionary<int, string>();
        Type tipoEnumTipo = typeof(enTipos);
        foreach (var valor in Enum.GetValues(tipoEnumTipo))
        {
            tipos.Add((int)valor, Enum.GetName(tipoEnumTipo, valor)!);
        }
        return tipos;
    }

}