using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstoqueController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEstoque([FromQuery] string? nomeItem, string? codigoReferenciaItem)
        {
            try
            { 
                if (!String.IsNullOrEmpty(nomeItem) || !String.IsNullOrEmpty(codigoReferenciaItem))
                {
                    var lista = from a in await _context.Estoque.Where(e => e.Item.Nome == nomeItem || e.Item.CodigoReferencia == codigoReferenciaItem).ToListAsync()
                                join b in _context.Item on a.Item equals b
                                select new EstoqueDTO { ItemNome = b.Nome, ItemCodigoReferencia = b.CodigoReferencia, Quantidade = a.Quantidade };

                    if (lista == null)                   
                        return NotFound($"Item não encontrado no estoque");

                    return Ok(lista);
                }
                else
                {                
                    var lista = from a in await _context.Estoque.ToListAsync()
                                join b in _context.Item on a.Item equals b
                                select new EstoqueDTO {ItemNome = b.Nome, ItemCodigoReferencia = b.CodigoReferencia, Quantidade = a.Quantidade };

                    if (lista == null)
                        return NotFound($"Nenhum registro encontrado no estoque");

                    return Ok(lista);                  
                }                
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro de consulta a lista. Exception:{ex.Message}");
            }
        }
      
        [HttpPost]
        public async Task<IActionResult> PostEstoque(EstoqueDTO request)
        {   
            try
            {
                var item = await _context.Item.Where(e => e.CodigoReferencia == request.ItemCodigoReferencia).FirstOrDefaultAsync();

                if (item == null)
                    return BadRequest($"Item não encontrado com o código de referência {request.ItemCodigoReferencia}, consute os itens disponíveis para incluir no estoque");
                
                var estoqueAux = await _context.Estoque.Where(e => e.Item.CodigoReferencia == request.ItemCodigoReferencia).FirstOrDefaultAsync();

                if (estoqueAux == null)
                {
                    var newEstoque = new Estoque
                    {
                        Item = item,
                        Quantidade = request.Quantidade
                    };

                    await _context.Estoque.AddAsync(newEstoque);
                    var valor = await _context.SaveChangesAsync();

                    if (valor > 0)
                        return Ok(request);
                    else                  
                        return BadRequest("Estoque não cadastrado.");
                }
                else
                {
                    estoqueAux.Quantidade += request.Quantidade;
                    _context.Estoque.Update(estoqueAux);

                    var valor = await _context.SaveChangesAsync();

                    if (valor > 0)                    
                        return Ok(estoqueAux);
                    else
                        return BadRequest("Estoque não alterado.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar estoque. Exception: {ex.Message}");
            }

        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetEstoquePaginacao([FromQuery] string? valor, int skip, int take, bool estoqueDesc)
        {
            try
            {
                var lista = from o in await _context.Estoque.ToListAsync()
                            select o;

                if (!String.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Item.ToString().Contains(valor)
                            select o;
                }

                if (estoqueDesc)
                {
                    lista = from o in lista
                            orderby o.Item.ToString() descending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Item.ToString() ascending
                            select o;
                }

                var qtde = lista.Count();

                lista = lista
                        .Skip((skip - 1) * take)
                        .Take(take)
                        .ToList();

                var paginacaoResponse = 
                    new PaginacaoResponse<Estoque>(lista, qtde, skip, take, estoqueDesc);

                return Ok(paginacaoResponse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro, pesquisa de estoque. Exceção: {ex.Message}");
            }
        }
    }
}
