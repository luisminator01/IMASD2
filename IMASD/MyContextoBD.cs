using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using IMASD.Models;
using IMASD.spReportes;
using IMASD;
using System.Linq;
using System.Threading.Tasks;

namespace IMASD
{
    public class MyContextoBD: DbContext
    {
        public MyContextoBD(DbContextOptions<MyContextoBD> options) : base(options)
        {

        }
        //          <clase>    Tabla
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Tabulador> Tabulador { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        public DbSet<spNominaList> spNominaList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<spNominaList>()
                .HasNoKey();
        }

    }
}
