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
        public async Task<IActionResult> GetItem([FromQuery] int? codigo, string? nome)
        {
            try
            {
                if (codigo > 0 || !String.IsNullOrEmpty(nome))
                {
                    var item = await _context.Item.Where(e => e.Codigo == codigo || e.Nome == nome).ToListAsync();

                    if (item == null)
                    {
                        return NotFound($"Item não encontrado com o código: {codigo}");
                    }

                    return Ok(item);
                }
                else
                {
                    var lista = await _context.Item.OrderBy(x => x.Codigo).ToListAsync();
                    return Ok(lista);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao consultar o item. Exception: {ex.Message}");
            } 
        }

        [HttpPost]
        public async Task<IActionResult> PostItem(Item item)
        {
            try
            {
                var itemAux = await _context.Item.Where(e => e.CodigoReferencia == item.CodigoReferencia).FirstOrDefaultAsync();

                if (itemAux != null)
                    return BadRequest($"Código de referência duplicado, " +
                                      $"já existe uma outra item cadastrado com o código de referência informado: {item.CodigoReferencia}" +
                                      Environment.NewLine + $"Nome: {item.Nome}");

                itemAux = await _context.Item.OrderByDescending(x => x.Codigo).FirstOrDefaultAsync();

                if (itemAux == null)
                    item.Codigo = 1;
                else
                    item.Codigo = itemAux.Codigo + 1;

                await _context.Item.AddAsync(item);
                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Ok("Item cadastrado com sucesso!");
                else
                    return BadRequest("Item não cadastrado.");
                
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar item. Exception: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutItem(Item item)
        {
            try
            {
                var itemAux = await _context.Item.Where(e => e.CodigoReferencia == item.CodigoReferencia || e.Codigo == item.Codigo).FirstOrDefaultAsync();

                if (itemAux == null)                
                    return NotFound($"Item não encontrado. Codigo de referência: {item.CodigoReferencia}");                

                itemAux.Nome = item.Nome;
                _context.Item.Update(itemAux);

                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Ok(item);
                else
                    return BadRequest("Item não alterado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao alterar item. Exception: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteItem([FromQuery] string codigoReferencia)
        {
            try
            {
                var itemAux = await _context.Item.Where(e => e.CodigoReferencia == codigoReferencia).FirstOrDefaultAsync();

                if (itemAux == null)
                    return NotFound($"Item não encontrado. Código de referência: {codigoReferencia}");

                _context.Item.Remove(itemAux);
                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Ok("Item excluído.");
                else
                    return BadRequest("Item não excluído.");
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
                var lista = from o in await _context.Item.ToListAsync()
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
