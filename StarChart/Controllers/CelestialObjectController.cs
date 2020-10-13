using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name ="GetById")]
        public  IActionResult  GetById(int id)
        {
            var result = _context.CelestialObjects.Find(id);

            if(result != null)
            {
                result.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();
                return Ok(result);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(n => n.Name == name);
                       
            if (result.Any())
            {
                foreach (var r in result)
                {
                    r.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == r.Id).ToList();
                }
                return Ok(result);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects.ToList();

            if (result.Any())
            {
                foreach (var r in result)
                {
                    r.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == r.Id).ToList();
                }
                return Ok(result);
            }
            else
            {
                return NotFound("There are no Celestial Objects");
            }

        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            if(_context.SaveChanges() > 0)
            {
                return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
            }
            else
            {
                return BadRequest("Could not Add Object");
             }

         }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var result = _context.CelestialObjects.Find(id);

            if(result != null)
            {
                result.Name = celestialObject.Name;
                result.OrbitalPeriod = celestialObject.OrbitalPeriod;
                result.OrbitedObjectId = celestialObject.OrbitedObjectId;

                _context.CelestialObjects.Update(result);

                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var result = _context.CelestialObjects.Find(id);

            if (result != null)
            {
                result.Name = name;
                _context.CelestialObjects.Update(result);
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _context.CelestialObjects.Where(i => i.Id == id || i.OrbitedObjectId == id);

            if (result.Any())
            {
                _context.RemoveRange(result);
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }

    }
}
