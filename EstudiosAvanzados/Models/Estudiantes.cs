using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EstudiosAvanzados.Models
{
    public partial class Estudiantes
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre no puede estar vacío.")]
        [StringLength(255, ErrorMessage = "Nombre no puede exceder 255 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Fecha de Nacimiento  no puede estar vacío.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Curso no puede estar vacío.")]
        [StringLength(255)]
        public string Curso { get; set; }

        public string Estado { get; set; }

        public int edad;
    }
}
