using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetOrdem()
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
        public async Task<IActionResult> GetOrdem([FromRoute] int codigo)
        {
            Ordem? ordem = new Ordem();

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
        public async Task<IActionResult> PostOrdem(OrdemDTO request)
        {
            try
            {
                var cliente = await _context.Pessoa.Where(e => e.Cpf == request.ClienteCPF).FirstOrDefaultAsync();

                if (cliente == null)
                    return BadRequest($"Cliente não encontrado com o CPF {request.ClienteCPF}, consulte se o cliente está cadastrado no sistema");

                var responsavel = await _context.Pessoa.Where(e => e.Cpf == request.ResponsavelCPF).FirstOrDefaultAsync();

                if (responsavel == null)
                    return BadRequest($"Reponsável não encontrado com o CPF {request.ResponsavelCPF}, consulte se o responsável está cadastrado no sistema");

                decimal valorLiquido = 0;
                decimal valorBruto = 0;
                decimal quantidadeServico = 0;
                decimal quantidadeItem = 0;
                decimal valorItem = 0;
                decimal valorServico = 0;
                int codigo = 0;

                var listItem = new List<OrdemItem>();
                var listServico = new List<OrdemServico>();
                
                foreach (var item in request.OrdemItem)
                {                    
                    var itemAux = await _context.Item.Where(e => e.Codigo == item.ItemCodigo).FirstOrDefaultAsync();

                    if (itemAux == null)
                        return BadRequest($"Item não encontrado com o código {item.ItemCodigo}, consulte os itens disponíveis para incluir na ordem");

                    OrdemItem ordemItem = new OrdemItem
                    {
                        Item = itemAux,
                        Observacao = item.Observacao,
                        Codigo = codigo,
                        Quantidade = item.Quantidade,
                        ValorUnitario = item.ValorUnitario
                    };

                    listItem.Add(ordemItem);

                    quantidadeItem++;
                    codigo++;

                    valorItem += item.ValorUnitario * item.Quantidade;
                }

                codigo = 0;

                foreach (var servico in request.OrdemServico)
                {
                    var servicoAux = await _context.Item.Where(e => e.Codigo == servico.ItemCodigo).FirstOrDefaultAsync();

                    if (servicoAux == null)
                        return BadRequest($"Item não encontrado com o código {servico.ItemCodigo}, consulte os itens disponíveis para incluir na ordem");

                    OrdemServico ordemServico = new OrdemServico
                    {
                        Servico = servicoAux,
                        Observacao = servico.Observacao,
                        Codigo = codigo,
                        Quantidade = servico.Quantidade,
                        ValorUnitario = servico.ValorUnitario
                    };

                    listServico.Add(ordemServico);

                    quantidadeServico++;

                    codigo++;

                    valorServico += servico.ValorUnitario * servico.Quantidade;
                }

                valorBruto = valorServico + valorLiquido;

                valorLiquido = valorBruto - (valorBruto * (request.Desconto / 100));

                Ordem ordem = new Ordem
                {
                    Cliente = cliente,
                    //Responsavel = responsavel,
                    ValorBruto = valorBruto,
                    ValorLiquido = valorLiquido,
                    QuantidadeServico = quantidadeServico,
                    QuantidadeItem = quantidadeItem,
                    ValorServico = valorServico,
                    ValorItem = valorItem,
                    Desconto = request.Desconto,

                    Tipo = request.Tipo,
                    Data = request.Data,
                    Observacao = request.Observacao,
                };

                var ordemAux = _context.Ordem.OrderByDescending(x => x.Codigo).FirstOrDefaultAsync();

                if (ordemAux == null) 
                    ordem.Codigo = 1;
                else              
                    ordem.Codigo++; 
                
                await _context.Ordem.AddAsync(ordem);                              

                foreach (var item in listItem)
                {
                    item.Ordem = ordem;
                    await _context.OrdemItem.AddAsync(item);
                }

                foreach (var servico in listServico)
                {
                    servico.Ordem = ordem;
                    await _context.OrdemServico.AddAsync(servico);
                }

                var valor = await _context.SaveChangesAsync();

                if (valor == 1)
                    return Ok("Ordem cadastrada com sucesso!");
                else
                    return BadRequest("Ordem não cadastrada.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar ordem. Exception: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutOrdem(Ordem ordem)
        {
            try
            {
                Ordem? ordemAux = await _context.Ordem.FindAsync(ordem.Codigo);

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
        public async Task<IActionResult> DeleteOrdemCodigo(int codigo)
        {
            try
            {
                Ordem? ordemAux = await _context.Ordem.FindAsync(codigo);

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
