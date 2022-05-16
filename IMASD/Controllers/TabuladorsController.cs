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
    public class TabuladorsController : ControllerBase
    {
        private readonly MyContextoBD _context;

        public TabuladorsController(MyContextoBD context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tabulador>>> GetTabuladores()
        {
            // return await _context.Tabulador.ToListAsync();
            return await _context.Tabulador
                  .FromSqlInterpolated($"EXEC GetTabulador").ToListAsync();
        }

        // GET 
        [HttpGet("{id}")]
        public async Task<ActionResult<Tabulador>> GetTabulador(int id)
        {
            /*   var tabulador = await _context.Tabulador.FindAsync(id);

               if (tabulador == null)
               {
                   return NotFound();
               }
               return tabulador; */
            var tabs = _context.Tabulador
                        .FromSqlInterpolated($"EXEC GetTabuladorId @idTabulador={id}")
                        .AsAsyncEnumerable();

            await foreach (var tab in tabs)
            {
                return tab;
            }
            return NotFound();
        }


        // POST 
        [HttpPost]
        public async Task<ActionResult<Tabulador>> PostTabulador(Tabulador tabulador)
        {              //tabla  
                       //  _context.Tabulador.Add(tabulador);
                       //  await _context.SaveChangesAsync();

            var parametroId = new SqlParameter("@idTabulador", System.Data.SqlDbType.Int);
            parametroId.Direction = System.Data.ParameterDirection.Output;

            await _context.Database
                          .ExecuteSqlInterpolatedAsync($@"EXEC PostTabulador
                                                               @nivelTabulador={tabulador.nivelTabulador},
                                                               @sbruto={tabulador.sbruto},
                                                               @idTabulador={parametroId} Output");
            var idTabulador = (int)parametroId.Value;

            return CreatedAtAction("GetTabulador", new { id = tabulador.idTabulador }, tabulador);
        }


        // PUT 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTabulador(int id, Tabulador tabulador)
        {
            if (id != tabulador.idTabulador)
            {
                return BadRequest();
            }
            _context.Entry(tabulador).State = EntityState.Modified;

            try
            {
                //  await _context.SaveChangesAsync();

                await _context.Database
                        .ExecuteSqlInterpolatedAsync($@"EXEC PutTabulador
                                                                           @idTabulador={id},
                                                                           @nivelTabulador={tabulador.nivelTabulador},
                                                                           @sbruto ={tabulador.sbruto}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TabuladorExists(id))
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
        public async Task<ActionResult<Tabulador>> DeleteTabulador(int id)
        {
            var tabulador = await _context.Tabulador.FindAsync(id);
            if (tabulador == null)
            {
                return NotFound();
            }
            _context.Tabulador.Remove(tabulador);
            await _context.SaveChangesAsync();

            return tabulador;
        }

        private bool TabuladorExists(int id)
        {
            return _context.Tabulador.Any(e => e.idTabulador == id);
        }
    }
}
