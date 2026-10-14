
using Backend.DTOs; // Importa los DTOs que usamos para recibir y devolver datos.
using Backend.Models; // Importa los modelos de la base de datos.
using Microsoft.AspNetCore.Mvc; // Permite crear rutas y responder peticiones HTTP.
using Microsoft.EntityFrameworkCore; // Permite consultar y modificar la base de datos.

namespace Backend.Controllers
{
    // Crea la ruta principal del controlador: api/Beer.
    [Route("api/[controller]")]

    // Activa validaciones automáticas y facilita el manejo de las peticiones.
    [ApiController]

    // Esta clase controla las operaciones que podemos hacer con las cervezas.
    public class BeerController : ControllerBase
    {
        // Guarda la conexión con la base de datos para poder trabajar con ella.
        private readonly StoreContext _context;

        // Recibe el contexto de la base de datos cuando se crea el controlador.
        public BeerController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Beer
        // Obtiene todas las cervezas registradas en la base de datos.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerDto>>> Get()
        {
            // Consulta las cervezas y selecciona solo los datos que necesitamos.
            var beers = await _context.Beers
                .Select(b => new BeerDto
                {
                    Id = b.BeerId,       // Obtiene el ID de la cerveza.
                    Name = b.Name,       // Obtiene su nombre.
                    Alcohol = b.Alcohol, // Obtiene su porcentaje de alcohol.
                    BrandId = b.BrandId  // Obtiene el ID de su marca.
                })
                .ToListAsync(); // Ejecuta la consulta y guarda los resultados en una lista.

            // Devuelve la lista de cervezas con una respuesta HTTP 200 (OK).
            return Ok(beers);
        }

        // GET: api/Beer/5
        // Busca una cerveza específica usando su ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDto>> GetById(int id)
        {
            // Busca la cerveza cuyo ID coincida con el que recibimos.
            var beer = await _context.Beers
                .Where(b => b.BeerId == id)
                .Select(b => new BeerDto
                {
                    Id = b.BeerId,       // Guarda el ID de la cerveza.
                    Name = b.Name,       // Guarda el nombre de la cerveza.
                    Alcohol = b.Alcohol, // Guarda el porcentaje de alcohol.
                    BrandId = b.BrandId  // Guarda el ID de la marca.
                })
                .FirstOrDefaultAsync(); // Devuelve la primera coincidencia o null si no existe.

            // Si no se encontró la cerveza, responde con un error 404 (Not Found).
            if (beer == null)
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Cerveza no encontrada",
                    Detail = $"No existe una cerveza con el ID {id}."
                });

            // Si existe, devuelve sus datos con una respuesta 200 (OK).
            return Ok(new
            {
                mensaje = "Beer encontrada ",
                beer = beer
            });
        }

        // POST: api/Beer
        // Registra una nueva cerveza en la base de datos.
        [HttpPost]
        public async Task<ActionResult<BeerDto>> Post(BeerInsertDto dto)
        {
            // Crea una cerveza usando los datos que recibimos del cliente.
            var beer = new Beer
            {
                Name = dto.Name,       // Asigna el nombre recibido.
                Alcohol = dto.Alcohol, // Asigna el porcentaje de alcohol.
                BrandId = dto.BrandId  // Asigna el ID de la marca.
            };

            // Agrega la nueva cerveza a la base de datos.
            _context.Beers.Add(beer);

            // Guarda los cambios y registra la cerveza.
            await _context.SaveChangesAsync();

            // Prepara los datos de la cerveza recién creada para devolverlos.
            var result = new BeerDto
            {
                Id = beer.BeerId,      // Obtiene el ID generado al guardar.
                Name = beer.Name,      // Obtiene el nombre registrado.
                Alcohol = beer.Alcohol,// Obtiene el porcentaje de alcohol.
                BrandId = beer.BrandId // Obtiene el ID de la marca.
            };

            // Devuelve una respuesta 201 (Created) con los datos creados.
            // También indica la ruta donde se puede consultar esa cerveza.
            return CreatedAtAction(
                nameof(GetById),
                new { id = beer.BeerId },
                new
                {
                    mensaje="Beer creada Corectamente",
                    beer = result
                }
                
            );
        }

        // PUT: api/Beer/5
        // Actualiza los datos de una cerveza que ya existe.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, BeerUpdateDto dto)
        {
            // Busca en la base de datos la cerveza que queremos modificar.
            var beer = await _context.Beers.FindAsync(id);

            // Si no existe una cerveza con ese ID, responde con un error 404.
            if (beer == null)
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Cerveza no encontrada",
                    Detail = $"No existe una cerveza con el ID {id}."
                });

            // Actualiza los datos con la información que recibimos.
            beer.Name = dto.Name;
            beer.Alcohol = dto.Alcohol;
            beer.BrandId = dto.BrandId;

            // Guarda los cambios realizados en la base de datos.
            await _context.SaveChangesAsync();

            // Devuelve una respuesta 204 (No Content).
            // Significa que se actualizó correctamente y no hay contenido que devolver.
            //return NoContent();
            return Ok(new{
              mesage="Se actualizo corectamente"  
            });
        }

        // DELETE: api/Beer/5
        // Elimina una cerveza usando su ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Busca la cerveza que queremos eliminar.
            var beer = await _context.Beers.FindAsync(id);

            // Si no existe, responde con un error 404 (Not Found).
            if (beer == null)
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Cerveza no encontrada",
                    Detail = $"No existe una cerveza con el ID {id}."
                });

            // Marca la cerveza para eliminarla de la base de datos.
            _context.Beers.Remove(beer);

            // Guarda los cambios y realiza la eliminación.
            await _context.SaveChangesAsync();

            // Devuelve una respuesta 204 porque se eliminó correctamente.
           // return NoContent();
             return Ok(new{
              mesage="Se elimino corectamente"  
            });
        }
    }
}
