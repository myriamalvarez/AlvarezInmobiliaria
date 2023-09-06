using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlvarezInmobiliaria.Models
{
    public class Pago
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "N° de Pago")]
        public int NumeroPago { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        public decimal Importe { get; set; }

        [Display(Name = "Contrato N°")]
        public int ContratoId { get; set; }

        public Contrato? Contrato { get; set; }
    }
}
