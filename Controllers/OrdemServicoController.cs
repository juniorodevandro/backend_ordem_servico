using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdemServicoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdemServicoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdemServico([FromQuery] int ordemCodigo)
        {
            try
            {
                var ordemServico = await _context.OrdemServico.
                                    //Include(x => x.Ordem).
                                    Where(e => e.Ordem.Codigo == ordemCodigo).ToListAsync();

                if (ordemServico == null)
                    return NotFound($"Ordem não encontrada com o código: {ordemCodigo}");

                return Ok(ordemServico);

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao consultar os serviços da ordem. Exception: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostOrdemServico(OrdemServicoDTO request)
        {
            try
            {
                var ordem = await _context.Ordem.Where(e => e.Codigo == request.CodigoOrdem).FirstOrDefaultAsync();

                if (ordem == null)
                    return BadRequest($"Código inválido, não existe uma ordem com o código informado {request.CodigoOrdem}");

                var servico = await _context.Item.Where(e => e.CodigoReferencia == request.ItemCodigoReferencia).FirstOrDefaultAsync();

                if (servico == null)
                    return BadRequest($"Código de referência inválido, não existe um item com o código de referência informado {request.ItemCodigoReferencia}");

                var ordemServicoAux = await _context.OrdemServico.OrderByDescending(x => x.Codigo).FirstOrDefaultAsync();

                int codigo = 1;

                if (ordemServicoAux != null)
                    codigo = ordemServicoAux.Codigo + 1;               

                var newOrdemServico = new OrdemServico()
                {
                    Ordem = ordem,
                    Servico = servico,
                    Quantidade = request.Quantidade,
                    ValorUnitario = request.ValorUnitario,
                    Observacao = request.Observacao,
                    Codigo = codigo
                };

                await _context.OrdemServico.AddAsync(newOrdemServico);
                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Ok("Serviço da ordem cadastrado com sucesso!");
                else
                    return BadRequest("Serviço da ordem não cadastrado.");
                
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar serviço da ordem. Exception: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutOrdemServico(OrdemServicoDTO request)
        {
            try
            {
                var ordemItem = await _context.OrdemServico.Where(e => e.Codigo == request.Codigo).FirstOrDefaultAsync();

                if (ordemItem == null)
                    return BadRequest($"Serviço da ordem inválido, não existe um item da ordem com o código informado {request.Codigo}");

                var ordem = await _context.Ordem.Where(e => e.Codigo == request.CodigoOrdem).FirstOrDefaultAsync();

                if (ordem == null)
                    return BadRequest($"Código inválido, não existe um ordem com o código informado {request.CodigoOrdem}");

                var item = await _context.Item.Where(e => e.CodigoReferencia == request.ItemCodigoReferencia).FirstOrDefaultAsync();

                if (item == null)
                    return BadRequest($"Código de referência inválido, não existe um item com o código de referência informado {request.ItemCodigoReferencia}");

                _context.OrdemServico.Update(ordemItem);

                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Ok(ordemItem);
                else
                    return BadRequest("Serviço da ordem não alterado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao alterar o item da ordem. Exception: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrdemServico([FromQuery] int codigo, int codigoOrdem)
        {
            try
            {
                var ordem = await _context.Ordem.Where(e => e.Codigo == codigoOrdem).FirstOrDefaultAsync();

                if (ordem == null)
                    return BadRequest($"Código inválido, não existe uma ordem com o código informado {codigoOrdem}");

                var ordemItem = await _context.OrdemServico.Where(e => e.Codigo == codigo).FirstOrDefaultAsync();

                if (ordemItem == null)
                    return NotFound($"Serviço da ordem não encontrado. Código: {codigo}");

                _context.OrdemServico.Remove(ordemItem);
                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Ok("Serviço da ordem excluído.");
                else
                    return BadRequest("Serviço da ordem não excluído.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir o serviço da ordem. Exception: {ex.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetOrdemServicoPaginacao([FromQuery] string? valor, int skip, int take, bool OrdemServicoDesc)
        {
            try
            {
                var lista = from o in await _context.OrdemServico.ToListAsync()
                            select o;

                if (!String.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Codigo.ToString().Contains(valor)
                            select o;
                }

                if (OrdemServicoDesc)
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
                    new PaginacaoResponse<OrdemServico>(lista, qtde, skip, take, OrdemServicoDesc);

                return Ok(paginacaoResponse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro, pesquisa do serviço da ordem. Exceção: {ex.Message}");
            }
        }
    }
}
