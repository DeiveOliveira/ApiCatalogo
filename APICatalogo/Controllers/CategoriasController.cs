using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            _logger.LogInformation("***************** GET  API/CATEGORIAS ***************** ");

            return _context.Categorias.AsNoTracking().ToList();
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("***************** GET  API/CATEGORIAS/PRODUTOS ***************** ");


            return _context.Categorias.Include(p => p.Produtos).Where(C => C.CategoriaId <= 3).ToList();
        }


        [HttpGet("{id:int}", Name = "ObterCategorias")]
        public ActionResult<Categoria> Get(int id)
        {
            _logger.LogInformation("***************** GET  API/CATEGORIAS/ID ***************** ");


            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);

            if (categoria == null) return NotFound("Categoria não encontrada...");

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            _logger.LogInformation("***************** POST  API/CATEGORIAS ***************** ");



            if (categoria is null) return BadRequest();

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                   new { id = categoria.CategoriaId }, categoria);
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            _logger.LogInformation("***************** PUT  API/CATEGORIAS/ID ***************** ");


            if (id != categoria.CategoriaId) return BadRequest("Categoria diferentes....");

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("***************** DELETE  API/CATEGORIAS/ID ***************** ");

            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            //Usando o Find, ele tenta localizar primeiro na memória, porem a chave deve ser uma Primary Key
            //var produto = _context.Produtos.Find(id);


            if (categoria is null) return NotFound("Produto não encontrado....");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);

        }

    }
}
