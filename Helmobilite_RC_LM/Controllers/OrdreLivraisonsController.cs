using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelmoBilite_RC_LM.Models;

namespace Helmobilite_RC_LM.Controllers
{
    public class OrdreLivraisonsController : Controller
    {
        private readonly HelmoBiliteRcLmContext _context;

        public OrdreLivraisonsController(HelmoBiliteRcLmContext context)
        {
            _context = context;
        }

        // GET: OrdreLivraisons
        public async Task<IActionResult> Index()
        {
              return _context.OrdreLivraisons != null ? 
                          View(await _context.OrdreLivraisons.ToListAsync()) :
                          Problem("Entity set 'HelmoBiliteRcLmContext.OrdreLivraisons'  is null.");
        }

        // GET: OrdreLivraisons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrdreLivraisons == null)
            {
                return NotFound();
            }

            var ordreLivraison = await _context.OrdreLivraisons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordreLivraison == null)
            {
                return NotFound();
            }

            return View(ordreLivraison);
        }

        // GET: OrdreLivraisons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrdreLivraisons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] OrdreLivraison ordreLivraison)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordreLivraison);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ordreLivraison);
        }

        // GET: OrdreLivraisons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrdreLivraisons == null)
            {
                return NotFound();
            }

            var ordreLivraison = await _context.OrdreLivraisons.FindAsync(id);
            if (ordreLivraison == null)
            {
                return NotFound();
            }
            return View(ordreLivraison);
        }

        // POST: OrdreLivraisons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] OrdreLivraison ordreLivraison)
        {
            if (id != ordreLivraison.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordreLivraison);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdreLivraisonExists(ordreLivraison.Id))
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
            return View(ordreLivraison);
        }

        // GET: OrdreLivraisons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrdreLivraisons == null)
            {
                return NotFound();
            }

            var ordreLivraison = await _context.OrdreLivraisons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordreLivraison == null)
            {
                return NotFound();
            }

            return View(ordreLivraison);
        }

        // POST: OrdreLivraisons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrdreLivraisons == null)
            {
                return Problem("Entity set 'HelmoBiliteRcLmContext.OrdreLivraisons'  is null.");
            }
            var ordreLivraison = await _context.OrdreLivraisons.FindAsync(id);
            if (ordreLivraison != null)
            {
                _context.OrdreLivraisons.Remove(ordreLivraison);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdreLivraisonExists(int id)
        {
          return (_context.OrdreLivraisons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
