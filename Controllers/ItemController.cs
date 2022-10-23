using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getItem([FromQuery] int? codigo, string? nome)
        {
            Item item = new Item();

            try
            {
                if (codigo > 0 || !String.IsNullOrEmpty(nome))
                {
                    item = await _context.Item.Where(e => e.Codigo == codigo || e.Nome == nome).FirstOrDefaultAsync();

                    if (item == null)
                    {
                        return NotFound($"Item não encontrada com o código: {codigo}");
                    }

                    return Ok(item);
                }
                else
                {
                    var lista = _context.Item.OrderBy(x => x.Nome);
                    return Ok(lista);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao consultar a item. Exception: {ex.Message}");
            } 
        }

        [HttpPost]
        public async Task<IActionResult> postItem(Item item)
        {
            try
            {
                Pessoa pessoa = await _context.Pessoa.Where(e => e.Codigo == item.Cliente.Codigo).FirstOrDefaultAsync();

                if (pessoa == null)
                {
                    return BadRequest("Não foi encontrado o cliente com esse código.");
                }
                else
                {
                    item.Cliente = pessoa;

                    await _context.Item.AddAsync(item);
                    var valor = await _context.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok(item);
                    }
                    else
                    {
                        return BadRequest("Item não cadastrada.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar item. Exception: {ex.Message}");
            }

        }

        [HttpPut]
        public async Task<IActionResult> putItem(Item item)
        {
            try
            {
                Item itemAux = await _context.Item.Where(e => e.Codigo == item.Codigo).FirstOrDefaultAsync();

                if (itemAux == null)
                {
                    return NotFound($"Item não encontrada. Nome: {item.Nome}");
                }

                itemAux.Nome = item.Nome;
                _context.Item.Update(itemAux);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok(item);
                }
                else
                {
                    return BadRequest("Item não alterada.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao alterar item. Exception: {ex.Message}");
            }

        }

        [HttpDelete]
        public async Task<IActionResult> deleteItem([FromQuery] int codigo)
        {
            try
            {
                Item itemAux = await _context.Item.Where(e => e.Codigo == codigo).FirstOrDefaultAsync();

                if (itemAux == null)
                {
                    return NotFound($"Item não encontrada. Código: {codigo}");
                }

                _context.Item.Remove(itemAux);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Item excluída.");
                }
                else
                {
                    return BadRequest("Item não excluída.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir item. Exception: {ex.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetItemPaginacao([FromQuery] string? valor, int skip, int take, bool itemDesc)
        {
            try
            {
                var lista = from o in _context.Item.ToList()
                            select o;

                if (!String.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Nome.ToString().Contains(valor)
                            select o;
                }

                if (itemDesc)
                {
                    lista = from o in lista
                            orderby o.Nome.ToString() descending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Nome.ToString() ascending
                            select o;
                }

                var qtde = lista.Count();

                lista = lista
                        .Skip((skip - 1) * take)
                        .Take(take)
                        .ToList();

                var paginacaoResponse =
                    new PaginacaoResponse<Item>(lista, qtde, skip, take, itemDesc);

                return Ok(paginacaoResponse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro, pesquisa de item. Exceção: {ex.Message}");
            }
        }
    }
}
