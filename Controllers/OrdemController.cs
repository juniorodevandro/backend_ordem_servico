using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getOrdem()
        {
            try
            {
                var lista = _context.Ordem.OrderBy(x => x.Codigo);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro de consulta a lista. Exception:{ex.Message}");
            }
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> getOrdem([FromRoute] int codigo)
        {
            Ordem ordem = new Ordem();

            try
            {
                ordem = await _context.Ordem.FindAsync(codigo);

                if (ordem == null)
                {
                    return NotFound($"Ordem não encontrado com o codigo: {codigo}");
                }

                return Ok(codigo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao consultar a ordem. Exception: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> postOrdem(Ordem ordem)
        {
            try
            {
                await _context.Ordem.AddAsync(ordem);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok(ordem);
                }
                else
                {
                    return BadRequest("Ordem não cadastrada.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar ordem. Exception: {ex.Message}");
            }

        }

        [HttpPut]
        public async Task<IActionResult> putOrdem(Ordem ordem)
        {

            try
            {
                Ordem ordemAux = await _context.Ordem.FindAsync(ordem.Codigo);

                if (ordemAux == null)
                {
                    return NotFound($"Ordem não encontrada. Codigo: {ordem.Codigo}");
                }

                ordemAux.Codigo = ordem.Codigo;
                _context.Ordem.Update(ordemAux);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok(ordem);
                }
                else
                {
                    return BadRequest("Ordem não alterada.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao alterar ordem. Exception: {ex.Message}");
            }

        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> deleteOrdemCodigo(int codigo)
        {
            try
            {
                Ordem ordemAux = await _context.Ordem.FindAsync(codigo);

                if (ordemAux == null)
                {
                    return NotFound($"Ordem não encontrada. Codigo: {codigo}");
                }

                _context.Ordem.Remove(ordemAux);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Ordem excluída.");
                }
                else
                {
                    return BadRequest("Ordem não excluída.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir ordem. Exception: {ex.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetOrdemPaginacao([FromQuery] string? valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                var lista = from o in _context.Ordem.ToList()
                            select o;

                if (!String.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Codigo.ToString().Contains(valor)
                            select o;
                }

                if (ordemDesc)
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
                    new PaginacaoResponse<Ordem>(lista, qtde, skip, take, ordemDesc);

                return Ok(paginacaoResponse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro, pesquisa de ordem. Exceção: {ex.Message}");
            }
        }
    }
}
