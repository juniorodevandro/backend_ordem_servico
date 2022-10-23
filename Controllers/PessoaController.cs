﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {  
        private readonly AppDbContext _context;

        public PessoaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getPessoa([FromQuery] int? codigo, string? nome, string? cpf)
        {
            Pessoa pessoa = new Pessoa();

            try
            {
                if (codigo > 0 || !String.IsNullOrEmpty(nome) || !String.IsNullOrEmpty(cpf))
                {
                    pessoa = await _context.Pessoa.Where(e => e.Codigo == codigo || e.Nome == nome || e.Cpf == cpf).FirstOrDefaultAsync();

                    if (pessoa == null)
                    {
                        return NotFound($"Pessoa não encontrada com o código: {codigo}");
                    }

                    return Ok(pessoa);
                }
                else
                {
                    var list = _context.Pessoa.OrderBy(x => x.Nome);

                    return Ok(list);
                }               

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao consultar a pessoa. Exception: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> postPessoa(Pessoa pessoa)
        {
            try
            {
                await _context.Pessoa.AddAsync(pessoa);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok(pessoa);
                }
                else
                {
                    return BadRequest("Pessoa não cadastrada.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar pessoa. Exception: {ex.Message}");
            }

        }

        [HttpPut]
        public async Task<IActionResult> putPessoa(Pessoa pessoa)
        {
            try
            {
                Pessoa pessoaAux = await _context.Pessoa.Where(e => e.Codigo == pessoa.Codigo).FirstOrDefaultAsync();

                if (pessoaAux == null)
                {
                    return NotFound($"Pessoa não encontrada. Nome: {pessoa.Nome}");
                }

                pessoaAux.Nome = pessoa.Nome;
                _context.Pessoa.Update(pessoaAux);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok(pessoa);
                }
                else
                {
                    return BadRequest("Pessoa não alterada.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao alterar pessoa. Exception: {ex.Message}");
            }

        }

        [HttpDelete]
        public async Task<IActionResult> deletePessoa([FromQuery] int codigo)
        {
            try
            {
                Pessoa pessoaAux = await _context.Pessoa.Where(e => e.Codigo == codigo).FirstOrDefaultAsync();

                if (pessoaAux == null)
                {
                    return NotFound($"Pessoa não encontrada. Código: {codigo}");
                }

                _context.Pessoa.Remove(pessoaAux);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Pessoa excluída.");
                }
                else
                {
                    return BadRequest("Pessoa não excluída.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir pessoa. Exception: {ex.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetPessoaPaginacao([FromQuery] string? valor, int skip, int take, bool pessoaDesc)
        {
            try
            {
                var lista = from o in _context.Pessoa.ToList()
                            select o;

                if (!String.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Nome.ToString().Contains(valor)
                            select o;
                }

                if (pessoaDesc)
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
                    new PaginacaoResponse<Pessoa>(lista, qtde, skip, take, pessoaDesc);

                return Ok(paginacaoResponse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro, pesquisa de pessoa. Exceção: {ex.Message}");
            }
        }
    }
}
