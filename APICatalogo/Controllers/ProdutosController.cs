using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            try
            {
            var produtos = await _context.Produtos.AsNoTracking().ToListAsync();

            if(produtos is null) return NotFound("Produtos não encontrados.....");
            
            return produtos;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação!");
            }
        }

        [HttpGet("{id:int}", Name="ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {

            try
            {
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
           
            if (produto is null) return NotFound("Produto não encontrado...");
            
            return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação!");
            }
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            try
            {
            if (produto is null) return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                   new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação!");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            try
            {
            if (id != produto.ProdutoId) return BadRequest("Produtos diferentes");

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação!");
            }
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            try
            {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            
            //Usando o Find, ele tenta localizar primeiro na memória, porem a chave deve ser uma Primary Key
            //var produto = _context.Produtos.Find(id);


            if (produto is null) return NotFound("Produto não encontrado....");

            _context.Produtos.Remove(produto);  
            _context.SaveChanges();

            return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação!");
            }
        }

    }
}
