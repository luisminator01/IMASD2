using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMASD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace IMASD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly MyContextoBD _context;

        public EmpleadosController(MyContextoBD context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            // return await _context.Empleados.ToListAsync();
            return await _context.Empleados
                  .FromSqlInterpolated($"EXEC GetEmpleadosList").ToListAsync();
        }

        // GET 
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            /*  var empleado = await _context.Empleados.FindAsync(id);
              if (empleado == null)
              {
                  return NotFound();
              }
              return empleado; */

            var emps = _context.Empleados
                     .FromSqlInterpolated($"EXEC GetEmpleadosId @idEmpleado={id}")
                     .AsAsyncEnumerable();

            await foreach (var emp in emps)
            {
                return emp;
            }
            return NotFound();
        }


        // POST 
        [HttpPost]
        public async Task<ActionResult<Departamento>> PostEmpleados(Empleado empleado)
        {           //tabla  
            Console.WriteLine(empleado);
            int idDep = empleado.idDepartamento;
            int idTab = empleado.idTabulador;
            Console.WriteLine("idDep: " + idDep + " idTab: " + idTab);
            empleado.idDepartamento = idDep;
            empleado.idTabulador = idTab;
            //   _context.Empleados.Add(empleado);
            //   await _context.SaveChangesAsync();

            var parametroId = new SqlParameter("@idEmpleado", System.Data.SqlDbType.Int);
            parametroId.Direction = System.Data.ParameterDirection.Output;

            await _context.Database
                          .ExecuteSqlInterpolatedAsync($@"EXEC PostEmpleados
                                                               @nombreEmp={empleado.nombreEmp},
                                                               @apellidosEmp={empleado.apellidosEmp},
                                                               @direccionEmp={empleado.direccionEmp},
                                                               @telefonoEmp={empleado.telefonoEmp},
                                                               @estatusEmp={empleado.estatusEmp},
                                                               @idDepartamento={empleado.idDepartamento},
                                                               @idTabulador={empleado.idTabulador},
                                                               @idEmpleado={parametroId} Output");
            //var idTabulador = (int)parametroId.Value;

            return CreatedAtAction("GetEmpleados", new { id = empleado.idEmpleado }, empleado);
        }


        // PUT 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpleado(int id, Empleado empleado)
        {
            if (id != empleado.idEmpleado)
            {
                return BadRequest();
            }
            _context.Entry(empleado).State = EntityState.Modified;

            try
            {
                //  await _context.SaveChangesAsync();


                await _context.Database
                          .ExecuteSqlInterpolatedAsync($@"EXEC PutEmpleados
                                                                           @idEmpleado={id},
                                                                           @nombreEmp={empleado.nombreEmp},
                                                                           @apellidosEmp={empleado.apellidosEmp},
                                                                           @direccionEmp={empleado.direccionEmp},
                                                                           @telefonoEmp={empleado.telefonoEmp},
                                                                           @estatusEmp={empleado.estatusEmp},
                                                                           @idDepartamento={empleado.idDepartamento},
                                                                           @idTabulador={empleado.idTabulador} ");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Empleado>> DeleteEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();

            return empleado;
        }

        private bool EmpleadosExists(int id)
        {
            return _context.Empleados.Any(e => e.idEmpleado == id);
        }
    }
}
