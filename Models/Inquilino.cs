using System.ComponentModel.DataAnnotations;

namespace AlvarezInmobiliaria.Models
{
    public class Inquilino
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Apellido { get; set; } = "";

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Dni { get; set; } = "";

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Telefono { get; set; } = "";

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Email { get; set; } = "";

        public override string ToString() => $"{Nombre} {Apellido}";
    }
}
