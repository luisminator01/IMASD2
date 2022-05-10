using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMASD.Models
{
    public class Tabulador
    {
        [Key]
        public int idTabulador { get; set; }
        [Required]
        [Column(TypeName = "varchar(1)")]
        public char nivelTabulador { get; set; }
        public int sbruto { get; set; }
    }
}
