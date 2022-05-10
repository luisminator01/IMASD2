using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IMASD.Models
{
    public class Departamento
    {
        [Key]
        public int idDepartamento { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public String nombreDep { get; set; }
    }
}
