using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class OrdemsController : Controller
    {
        private readonly AppDbContext _context;

        public OrdemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Ordems
        public async Task<IActionResult> Index()
        {
              return View(await _context.Ordem.ToListAsync());
        }

        // GET: Ordems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Ordem == null)
            {
                return NotFound();
            }

            var ordem = await _context.Ordem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordem == null)
            {
                return NotFound();
            }

            return View(ordem);
        }

        // GET: Ordems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ordems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Data,ValorLiquido,ValorBruto,Tipo,Desconto,Observacao,ClienteCodigo,ResponsavelCodigo")] Ordem ordem)
        {
            if (ModelState.IsValid)
            {
                ordem.Id = Guid.NewGuid();
                _context.Add(ordem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ordem);
        }

        // GET: Ordems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Ordem == null)
            {
                return NotFound();
            }

            var ordem = await _context.Ordem.FindAsync(id);
            if (ordem == null)
            {
                return NotFound();
            }
            return View(ordem);
        }

        // POST: Ordems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Codigo,Data,ValorLiquido,ValorBruto,Tipo,Desconto,Observacao,ClienteCodigo,ResponsavelCodigo")] Ordem ordem)
        {
            if (id != ordem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdemExists(ordem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ordem);
        }

        // GET: Ordems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Ordem == null)
            {
                return NotFound();
            }

            var ordem = await _context.Ordem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordem == null)
            {
                return NotFound();
            }

            return View(ordem);
        }

        // POST: Ordems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Ordem == null)
            {
                return Problem("Entity set 'AppDbContext.Ordem'  is null.");
            }
            var ordem = await _context.Ordem.FindAsync(id);
            if (ordem != null)
            {
                _context.Ordem.Remove(ordem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdemExists(Guid id)
        {
          return _context.Ordem.Any(e => e.Id == id);
        }
    }
}
