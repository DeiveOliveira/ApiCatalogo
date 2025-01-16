using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
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
            try
            {
               return _context.Categorias.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "***************** Erro ao obetr todas as categorias  GET  API/CATEGORIAS  *******************");

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação!");
            }
           
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos() 
        {
            _logger.LogInformation("***************** GET  API/CATEGORIAS/PRODUTOS ***************** ");

            try
            {
            return _context.Categorias.Include(p => p.Produtos).Where(C => C.CategoriaId <= 3).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "***************** Erro ao obetr categorias e seus produtos  GET  API/CATEGORIAS/PRODUTOS *******************");


                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação!");
            }
        }


        [HttpGet("{id:int}", Name = "ObterCategorias")]
        public ActionResult<Categoria> Get(int id)
        {
            _logger.LogInformation("***************** GET  API/CATEGORIAS/ID ***************** ");

            try
            {
            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);
        
            if (categoria == null) return NotFound("Categoria não encontrada...");

            return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "***************** Erro ao obetr categoria por ID  GET  API/CATEGORIAS/ID *******************");

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação!");
            }
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            _logger.LogInformation("***************** POST  API/CATEGORIAS ***************** ");


            try
            {
            if (categoria is null) return BadRequest();

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                   new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "***************** Erro ao Salvar CATEGORIA  POST API/CATEGORIAS *******************");


                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação!");
            }
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            _logger.LogInformation("***************** PUT  API/CATEGORIAS/ID ***************** ");

            try
            {
            if (id != categoria.CategoriaId) return BadRequest("Categoria diferentes....");

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "***************** Erro ao tentar ATUALIZAR categoria  PUT API/CATEGORIAS/ID *******************");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação!");
            }

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("***************** DELETE  API/CATEGORIAS/ID ***************** ");

            try
            {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            //Usando o Find, ele tenta localizar primeiro na memória, porem a chave deve ser uma Primary Key
            //var produto = _context.Produtos.Find(id);


            if (categoria is null) return NotFound("Produto não encontrado....");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "***************** Erro ao tentar EXCLUIR Categoria  DELETE  API/CATEGORIAS/ID *******************");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação!");
            }
        }

    }
}
