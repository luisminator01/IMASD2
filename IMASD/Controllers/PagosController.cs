using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMASD.Models;
using Microsoft.Data.SqlClient;

namespace IMASD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly MyContextoBD _context;

        public PagosController(MyContextoBD context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagos()
        {
            // return await _context.Pagos.ToListAsync();
            return await _context.Pagos
                  .FromSqlInterpolated($"EXEC GetPagos").ToListAsync();
        }

        // GET 
        [HttpGet("{id}")]
        public async Task<ActionResult<Pago>> GetPago(int id)
        {
            /*   var pago = await _context.Pagos.FindAsync(id);
               if (pago == null)
               {
                   return NotFound();
               }
               return pago;  */
            var pagos = _context.Pagos
               .FromSqlInterpolated($"EXEC GetPagosId @idPago={id}")
               .AsAsyncEnumerable();

            await foreach (var pago in pagos)
            {
                return pago;
            }
            return NotFound();
        }


        // POST 
        [HttpPost]
        public async Task<ActionResult<Pago>> PostPagos(Pago pago)
        {            //tabla  
                     // _context.Pagos.Add(pago);
                     // await _context.SaveChangesAsync();

            var parametroId = new SqlParameter("@idPago", System.Data.SqlDbType.Int);
            parametroId.Direction = System.Data.ParameterDirection.Output;

            await _context.Database
                          .ExecuteSqlInterpolatedAsync($@"EXEC PostPagos
                                                               @fPago={pago.fPago},
                                                               @idEmpleado={pago.idEmpleado},
                                                               @idPago={parametroId} Output");
            //var idPago = (int)parametroId.Value;

            return CreatedAtAction("GetPagos", new { id = pago.idPago }, pago);
        }


        // PUT 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPagos(int id, Pago pago)
        {
            if (id != pago.idPago)
            {
                return BadRequest();
            }
            _context.Entry(pago).State = EntityState.Modified;

            try
            {
                //  await _context.SaveChangesAsync();


                await _context.Database
                               .ExecuteSqlInterpolatedAsync($@"EXEC PutPagos
                                                                           @idPago={id},
                                                                           @fpago={pago.fPago},
                                                                           @idEmpleado ={pago.idEmpleado}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagosExists(id))
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
        public async Task<ActionResult<Pago>> DeletePago(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return pago;
        }

        private bool PagosExists(int id)
        {
            return _context.Pagos.Any(e => e.idPago == id);
        }
    }
}
