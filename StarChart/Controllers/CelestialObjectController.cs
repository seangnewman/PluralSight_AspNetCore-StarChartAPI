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





    }
}
