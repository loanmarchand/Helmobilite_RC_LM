using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelmoBilite_RC_LM.Models;
using Helmobilite_RC_LM.ViewsModels;

namespace Helmobilite_RC_LM.Controllers
{
    public class LivraisonsController : Controller
    {
        private readonly HelmoBiliteRcLmContext _context;

        public LivraisonsController(HelmoBiliteRcLmContext context)
        {
            _context = context;
        }

        // GET: Livraisons
        public async Task<IActionResult> Index()
        {
            var helmoBiliteRcLmContext = _context.Livraisons.Include(l => l.Client).Include(l => l.LieuChargement).Include(l => l.LieuDechargement);
            return View(await helmoBiliteRcLmContext.ToListAsync());
        }

        // GET: Livraisons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Livraisons == null)
            {
                return NotFound();
            }

            var livraison = await _context.Livraisons
                .Include(l => l.Client)
                .Include(l => l.LieuChargement)
                .Include(l => l.LieuDechargement)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livraison == null)
            {
                return NotFound();
            }

            return View(livraison);
        }

        // GET: Livraisons/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["LieuChargementId"] = new SelectList(_context.Adresses, "Id", "Pays");
            ViewData["LieuDechargementId"] = new SelectList(_context.Adresses, "Id", "Pays");
            return View();
        }

        // POST: Livraisons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LivraisonsViewsModels livraisonsViewsModels)
        {
            if (ModelState.IsValid)
            {
                var livraison = new Livraison()
                {
                    Contenu = livraisonsViewsModels.Contenu,
                    DateHeureChargement = livraisonsViewsModels.DateChargement,
                    DateHeureDechargement = livraisonsViewsModels.DateDechargement,
                    Statut = "En attente"
                };
                //ajouter les lieux de chargement et de déchargement en base de données
                
                if (livraison.CheckDate())
                {
                    //Vérifie si un utilisateur est connecté et si c'est le cas, récupère son id
                    //Vérifie si un utilisateur est connecté et si c'est le cas, récupère son id
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // this line gets the user ID
                        livraison.ClientId = userId;
                        var lieuChargement = livraisonsViewsModels.LieuChargement;
                        var lieuDechargement = livraisonsViewsModels.LieuDechargement;
                        _context.Adresses.Add(lieuChargement);
                        _context.Adresses.Add(lieuDechargement);
                        await _context.SaveChangesAsync();
                        //ajouter les lieux de chargement et de déchargement à la livraison
                        livraison.LieuChargementId = lieuChargement.Id;
                        livraison.LieuDechargementId = lieuDechargement.Id;
                        _context.Add(livraison);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                
            }
            return View(livraisonsViewsModels);
        }

        // GET: Livraisons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Livraisons == null)
            {
                return NotFound();
            }

            var livraison = await _context.Livraisons.FindAsync(id);
            if (livraison == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", livraison.ClientId);
            ViewData["LieuChargementId"] = new SelectList(_context.Adresses, "Id", "Pays", livraison.LieuChargementId);
            ViewData["LieuDechargementId"] = new SelectList(_context.Adresses, "Id", "Pays", livraison.LieuDechargementId);
            return View(livraison);
        }

        // POST: Livraisons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LieuChargementId,LieuDechargementId,Contenu,DateHeureChargement,DateHeureDechargement,Statut,ClientId")] Livraison livraison)
        {
            if (id != livraison.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livraison);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivraisonExists(livraison.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", livraison.ClientId);
            ViewData["LieuChargementId"] = new SelectList(_context.Adresses, "Id", "Pays", livraison.LieuChargementId);
            ViewData["LieuDechargementId"] = new SelectList(_context.Adresses, "Id", "Pays", livraison.LieuDechargementId);
            return View(livraison);
        }

        // GET: Livraisons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Livraisons == null)
            {
                return NotFound();
            }

            var livraison = await _context.Livraisons
                .Include(l => l.Client)
                .Include(l => l.LieuChargement)
                .Include(l => l.LieuDechargement)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livraison == null)
            {
                return NotFound();
            }

            return View(livraison);
        }

        // POST: Livraisons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Livraisons == null)
            {
                return Problem("Entity set 'HelmoBiliteRcLmContext.Livraisons'  is null.");
            }
            var livraison = await _context.Livraisons.FindAsync(id);
            if (livraison != null)
            {
                _context.Livraisons.Remove(livraison);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivraisonExists(int id)
        {
          return (_context.Livraisons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
