using ITSTEPASPNET.Data;
using ITSTEPASPNET.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Security.AccessControl;

namespace ITSTEPASPNET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutosController : ControllerBase
    {

        private readonly ILogger<AutosController> _logger;
        private readonly AutoContext _dbConext;
        public AutosController(ILogger<AutosController> logger, AutoContext weather)
        {
            _dbConext = weather;
            _logger = logger;
        }

        public IActionResult Index(int Id)
        {
            List<Auto> person = (from m in _dbConext.autos select m).ToList();

            return View(person);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auto>>> Get()
        {
            return await _dbConext.autos.Select( x => AutoOut(x)).ToListAsync();
        }


        [HttpGet("Autos")]
        public async Task<ActionResult<Auto>> GetFindAuto(int Id)
        {
            var FindTemp = await _dbConext.autos.FindAsync(Id);
            if (FindTemp == null)
            {
                return NotFound(); 
            }

            return AutoOut(FindTemp);

        }

        [HttpPost(Name = "PostAuto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(Auto auto)
        {
            await _dbConext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { mark = auto.mark_auto, color = auto.color_auto, date = auto.price, price = auto.price, type = auto.type_auto, engine = auto.engine, isKaz = auto.isKazakstan, desc = auto.decs }, auto);
        }

        [HttpDelete("Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int Id)
        {
            var findDeleteObject = await _dbConext.autos.FindAsync(Id);
            if (findDeleteObject == null)
            {
                return NotFound();
            }
            _dbConext.autos.Remove(findDeleteObject);
            await _dbConext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("Id")]
        /*[ProducesResponseType(StatusCodes.Status204NoContent)]*/
        public async Task<ActionResult> Put(int Id, Auto auto)
        {
            if (Id == auto.Id)
            {

                var autoObject = _dbConext.autos.FindAsync(Id);

                try
                {
                    if (await autoObject != null)
                    {
                        _dbConext.Entry(autoObject).State = EntityState.Modified;
                        await _dbConext.SaveChangesAsync();
                    }
                    else
                    {
                        _dbConext.autos.Add(auto);
                    }

                    await _dbConext.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                   if (Id != auto.Id)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }
                
            }
            else
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPatch("Id")]
        public async Task<ActionResult> Patch(int Id, Auto auto)
        {
            var autoID = await _dbConext.autos.SingleAsync(x => x.Id == Id);

            autoID.mark_auto = auto.mark_auto;
            autoID.color_auto = auto.color_auto;
            autoID.date = auto.date;
            autoID.price = auto.price;
            autoID.type_auto = auto.type_auto;
            autoID.engine = auto.engine;
            autoID.isKazakstan = auto.isKazakstan;
            autoID.decs = auto.decs;

            await _dbConext.SaveChangesAsync();

            return NoContent();
        }




        private static Auto AutoOut(Auto auto) => new Auto {
                Id = auto.Id,
                mark_auto = auto.mark_auto,
                color_auto = auto.color_auto,
                date = auto.date,
                price = auto.price,
                type_auto = auto.type_auto,
                engine = auto.engine,
                isKazakstan = auto.isKazakstan,
                decs = auto.decs
                
        };
           

    }
}