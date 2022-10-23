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
        public async Task<IActionResult> getEstoque([FromQuery] string? nomeItem)
        {
            try
            {
                if (!String.IsNullOrEmpty(nomeItem))
                {
                    var estoque = await _context.Estoque.Where(e => e.Item.Nome == nomeItem).FirstOrDefaultAsync();

                    if (estoque == null)
                    {
                        return NotFound($"Item não encontrado no estoque");
                    }

                    return Ok(estoque);
                }
                else
                {
                    var lista = _context.Estoque.OrderBy(x => x.Item.Nome);
                    return Ok(lista);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro de consulta a lista. Exception:{ex.Message}");
            }
        }

      
        [HttpPost]
        public async Task<IActionResult> postEstoque(Estoque estoque)
        {
            try
            {
                await _context.Estoque.AddAsync(estoque);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok(estoque);
                }
                else
                {
                    return BadRequest("Estoque não cadastrada.");
                }       
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar estoque. Exception: {ex.Message}");
            }

        }

        [HttpPut]
        public async Task<IActionResult> putEstoque(Estoque estoque)
        {
            try
            {
                Estoque estoqueAux = await _context.Estoque.Where(e => e.Item == estoque.Item).FirstOrDefaultAsync();

                if (estoqueAux == null)
                {
                    return NotFound($"Estoque não encontrado para o produto. Nome: {estoque.Item.Nome}");
                }

                estoqueAux.Item = estoque.Item;
                _context.Estoque.Update(estoqueAux);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok(estoque);
                }
                else
                {
                    return BadRequest("Estoque não alterado.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao alterar estoque. Exception: {ex.Message}");
            }

        }        

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetEstoquePaginacao([FromQuery] string? valor, int skip, int take, bool estoqueDesc)
        {
            try
            {
                var lista = from o in _context.Estoque.ToList()
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
