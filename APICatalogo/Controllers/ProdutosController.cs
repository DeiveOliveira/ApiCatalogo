using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public ProdutosController(AppDbContext context, ILogger<ProdutosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            _logger.LogInformation("***************** GET  API/PRODUTOS ***************** ");

            var produtos = await _context.Produtos.AsNoTracking().ToListAsync();

            if (produtos is null) return NotFound("Produtos não encontrados.....");

            return produtos;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            _logger.LogInformation("***************** GET  API/PRODUTOS/ID ***************** ");

            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produto is null) return NotFound("Produto não encontrado...");

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            _logger.LogInformation("***************** POST  API/PRODUTOS ***************** ");

            if (produto is null) return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                   new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            _logger.LogInformation("***************** PUT  API/PRODUTOS/ID ***************** ");

            if (id != produto.ProdutoId) return BadRequest("Produtos diferentes");

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("***************** DELETE  API/PRODUTOS/ID ***************** ");

            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            //Usando o Find, ele tenta localizar primeiro na memória, porem a chave deve ser uma Primary Key
            //var produto = _context.Produtos.Find(id);


            if (produto is null) return NotFound("Produto não encontrado....");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }

    }
}
