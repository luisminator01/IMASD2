using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMASD;

namespace IMASD.Models
{
    public class Empleado
    {
        [Key]
        public int idEmpleado { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string nombreEmp { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string apellidosEmp { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string direccionEmp { get; set; }
        public string telefonoEmp { get; set; }
        [Required]
        public Boolean estatusEmp { get; set; }

        //clave externa para Departamentos
        [ForeignKey("Departamentos")]
        public int idDepartamento { get; set; }

        //Objeto que representa la clave externa
        [ForeignKey("idDepartamento")]
        public virtual Departamento Departamentos { get; set; }

        //clave externa para Tabulador
        [ForeignKey("Tabulador")]
        public int idTabulador { get; set; }

        //Objeto que representa la clave externa
        [ForeignKey("idTabulador")]
        public virtual Tabulador Tabulador { get; set; }

    }
}
