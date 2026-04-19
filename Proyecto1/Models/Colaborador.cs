using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class Colaborador
    {
        [Key]
        public int IdColaborador { get; set; }

        [Required(ErrorMessage = "La cedula es obligatoria")]
        [RegularExpression(@"^\d-\d{4}-\d{4}$", ErrorMessage = "Formato invalido. Use: 1-0000-0000")]
        [StringLength(20, ErrorMessage = "La cedula no puede superar 20 caracteres")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras")]
        [StringLength(80, ErrorMessage = "El nombre no puede superar 80 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Los apellidos solo pueden contener letras")]
        [StringLength(120, ErrorMessage = "Los apellidos no pueden superar 120 caracteres")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El puesto es obligatorio")]
        [StringLength(80, ErrorMessage = "El puesto no puede superar 80 caracteres")]
        public string Puesto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El departamento es obligatorio")]
        [StringLength(80, ErrorMessage = "El departamento no puede superar 80 caracteres")]
        public string Departamento { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria")]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "El salario base es obligatorio")]
        [Range(0.01, 99999999.99, ErrorMessage = "El salario debe ser mayor a 0")]
        public decimal SalarioBase { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo no es valido")]
        [StringLength(120, ErrorMessage = "El correo no puede superar 120 caracteres")]
        public string? Correo { get; set; }

        public bool Estado { get; set; }
    }
}
