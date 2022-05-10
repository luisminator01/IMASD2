using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IMASD.Models
{
    public class Pago
    {
        [Key]
        public int idPago { get; set; }
        public String fPago { get; set; }

        [ForeignKey("Empleados")]
        public int idEmpleado { get; set; }

        [ForeignKey("idEmpleado")]
        public virtual Empleado Empleados { get; set; }
    }
}
