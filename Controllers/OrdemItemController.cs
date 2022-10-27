using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdemItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdemItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdemItem([FromQuery] int ordemCodigo)
        {
            try
            {
                var ordemItem = await _context.OrdemItem.
                                      //Include(x => x.Ordem).
                                      Where(e => e.Ordem.Codigo == ordemCodigo).ToListAsync();

                if (ordemItem == null)
                    return NotFound($"Ordem não encontrada com o código: {ordemCodigo}");

                return Ok(ordemItem);

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao consultar os itens da ordem. Exception: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostOrdemItem(OrdemItemDTO request)
        {
            try
            {
                var ordem = await _context.Ordem.Where(e => e.Codigo == request.CodigoOrdem).FirstOrDefaultAsync();

                if (ordem == null)
                    return BadRequest($"Código inválido, não existe uma ordem com o código informado {request.CodigoOrdem}");

                var item = await _context.Item.Where(e => e.CodigoReferencia == request.ItemCodigoReferencia).FirstOrDefaultAsync();

                if (item == null)
                    return BadRequest($"Código de referência inválido, não existe um item com o código de referência informado {request.ItemCodigoReferencia}");

                var ordemItemAux = await _context.OrdemItem.OrderByDescending(x => x.Codigo).FirstOrDefaultAsync();

                int codigo = 1;

                if (ordemItemAux != null)
                    codigo = ordemItemAux.Codigo + 1;

                var newOrdemItem = new OrdemItem()
                {
                    Ordem = ordem,
                    Item = item,
                    Quantidade = request.Quantidade,
                    ValorUnitario = request.ValorUnitario,
                    Observacao = request.Observacao,
                    Codigo = codigo
                };

                await _context.OrdemItem.AddAsync(newOrdemItem);
                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Ok("Item da ordem cadastrado com sucesso!");
                else
                    return BadRequest("Item da ordem não cadastrado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar item da ordem. Exception: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutOrdemItem(OrdemItemDTO request)
        {
            try
            {
                var ordemItem = await _context.OrdemItem.Where(e => e.Codigo == request.Codigo).FirstOrDefaultAsync();

                if (ordemItem == null)
                    return BadRequest($"Item da ordem inválido, não existe um item da ordem com o código informado {request.Codigo}");

                var ordem = await _context.Ordem.Where(e => e.Codigo == request.CodigoOrdem).FirstOrDefaultAsync();

                if (ordem == null)
                    return BadRequest($"Código inválido, não existe uma ordem com o código informado {request.CodigoOrdem}");

                var item = await _context.Item.Where(e => e.CodigoReferencia == request.ItemCodigoReferencia).FirstOrDefaultAsync();

                if (item == null)
                    return BadRequest($"Código de referência inválido, não existe um item com o código de referência informado {request.ItemCodigoReferencia}");

                _context.OrdemItem.Update(ordemItem);

                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Ok(ordemItem);
                else
                    return BadRequest("Item da ordem não alterado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao alterar o item da ordem. Exception: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrdemItem([FromQuery] int codigo, int codigoOrdem)
        {
            try
            {
                var ordem = await _context.Ordem.Where(e => e.Codigo == codigoOrdem).FirstOrDefaultAsync();

                if (ordem == null)
                    return BadRequest($"Código inválido, não existe uma ordem com o código informado {codigoOrdem}");

                var ordemItem = await _context.OrdemItem.Where(e => e.Codigo == codigo).FirstOrDefaultAsync();

                if (ordemItem == null)
                    return NotFound($"Item da ordem não encontrado. Código: {codigo}");

                _context.OrdemItem.Remove(ordemItem);
                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Ok("Item da ordem excluído.");
                else
                    return BadRequest("Item da ordem não excluído.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir o item da ordem. Exception: {ex.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetOrdemItemPaginacao([FromQuery] string? valor, int skip, int take, bool OrdemItemDesc)
        {
            try
            {
                var lista = from o in await _context.OrdemItem.ToListAsync()
                            select o;

                if (!String.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Codigo.ToString().Contains(valor)
                            select o;
                }

                if (OrdemItemDesc)
                {
                    lista = from o in lista
                            orderby o.Codigo.ToString() descending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Codigo.ToString() ascending
                            select o;
                }

                var qtde = lista.Count();

                lista = lista
                        .Skip((skip - 1) * take)
                        .Take(take)
                        .ToList();

                var paginacaoResponse =
                    new PaginacaoResponse<OrdemItem>(lista, qtde, skip, take, OrdemItemDesc);

                return Ok(paginacaoResponse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro, pesquisa do item da ordem. Exceção: {ex.Message}");
            }
        }
    }
}
