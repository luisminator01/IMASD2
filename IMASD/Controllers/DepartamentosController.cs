using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMASD;
using IMASD.Models;
using Microsoft.Data.SqlClient;

namespace IMASD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        private readonly MyContextoBD _context;

        public DepartamentosController(MyContextoBD context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departamento>>> GetDepartamentos()
        {
            // return await _context.Departamentos.ToListAsync();
            return await _context.Departamentos
               .FromSqlInterpolated($"EXEC GetDepartamentos").ToListAsync();
        }

        // GET 
        [HttpGet("{id}")]
        public async Task<ActionResult<Departamento>> GetDepartamento(int id)
        {
            /*  var depto = await _context.Departamentos.FindAsync(id);

              if (depto == null)
              {
                  return NotFound();
              }
              return depto;   */
            var deptos = _context.Departamentos
                                 .FromSqlInterpolated($"EXEC GetDeptoId @idDepartamento={id}")
                                 .AsAsyncEnumerable();

            await foreach (var depto in deptos)
            {
                return depto;
            }
            return NotFound();

        }


        // POST 
        [HttpPost]
        public async Task<ActionResult<Departamento>> PostDepartamentos(Departamento departamentos)
        {           //tabla  
                    //  _context.Departamentos.Add(departamentos);
                    //   await _context.SaveChangesAsync();

            var parametroId = new SqlParameter("@idDepartamento", System.Data.SqlDbType.Int);
            parametroId.Direction = System.Data.ParameterDirection.Output;

            await _context.Database
                          .ExecuteSqlInterpolatedAsync($@"EXEC PostDepartamentos
                                                               @nombreDep={departamentos.nombreDep},
                                                               @idDepartamento={parametroId} Output");
            var idDepartamento = (int)parametroId.Value;

          //  return Ok(idDepartamento);

            return CreatedAtAction("GetDepartamentos", new { id = departamentos.idDepartamento }, departamentos);
        }


        // PUT 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartamentos(int id, Departamento departamentos)
        {
            if (id != departamentos.idDepartamento)
            {
                return BadRequest();
            }
         //   _context.Entry(departamentos).State = EntityState.Modified;

            try
            {
                //   await _context.SaveChangesAsync();

                // var parametroId = new SqlParameter("@idDepartamento", System.Data.SqlDbType.Int);
                // parametroId.Direction = System.Data.ParameterDirection.Output;

                    await _context.Database
                                .ExecuteSqlInterpolatedAsync($@"EXEC PutDepartamentos
                                                               @idDepartamento={id},
                                                               @nombreDep={departamentos.nombreDep} ");

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartamentosExists(id))
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
        public async Task<ActionResult<Departamento>> DeleteDepartamento(int id)
        {
            var depto = await _context.Departamentos.FindAsync(id);
            if (depto == null)
            {
                return NotFound();
            }
            _context.Departamentos.Remove(depto);
            await _context.SaveChangesAsync();

            return depto;
        }

        private bool DepartamentosExists(int id)
        {
            return _context.Departamentos.Any(e => e.idDepartamento == id);
        }
    }
}
