using System.ComponentModel.DataAnnotations;

namespace AlvarezInmobiliaria.Models;

    public class Contrato
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required (ErrorMessage = "La fecha de inicio es obligatoria")]
        [Display(Name = "Fecha de inicio")]
        public DateTime? FechaInicio { get; set; }

        [Required (ErrorMessage = "La fecha de finalización es obligatorio")]
        [Display(Name = "Fecha de finalización")]
        public DateTime? FechaFin { get; set; }

        [Required (ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Monto mensual")]
        public decimal Alquiler { get; set; }

        [Required (ErrorMessage = "El inmueble es obligatorio")]
        [Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }

        public Inmueble? Inmueble { get; set; }

        [Required (ErrorMessage ="El inquilino es obligatorio")]
        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }

        public Inquilino? Inquilino { get; set; }

        public string? Estado { get; set; }
    }