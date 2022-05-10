using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMASD.spReportes;

namespace IMASD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpNominaController : ControllerBase
    {
        private readonly MyContextoBD _context;

        public SpNominaController(MyContextoBD context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<spNominaList>>> GetNominaList()
        {
            //return await _context.<Tabla>.ToListAsync();

            return await _context.spNominaList
                .FromSqlInterpolated($"EXEC GetNominaList").ToListAsync(); ;
/*
            await foreach (var nom in noms)
            {
                return nom;
            }
            return NotFound(); */
        }

        // GET 
        [HttpGet("{ide}/{idt}")]
        public async Task<ActionResult<spNominaList>> GetNominaList(int ide, int idt)
        {
            /*  var depto = await _context.Departamentos.FindAsync(id);

              if (depto == null)
              {
                  return NotFound();
              }
              return depto;   */
            Console.WriteLine("id dep: " + ide + " id tab: " + idt);
            var emps = _context.spNominaList
                                 .FromSqlInterpolated($"EXEC GetNominaFiltros @idDepartamento={ide} @idTabulador={idt}")
                                 .AsAsyncEnumerable();

            await foreach (var emp in emps)
            {
                return emp;
            }
            return NotFound();

        }

    }
}
