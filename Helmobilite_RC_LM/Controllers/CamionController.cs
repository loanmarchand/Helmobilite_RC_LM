using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelmoBilite_RC_LM.Models;
using Microsoft.AspNetCore.Authorization;


namespace HelmoBilite_RC_LM.Controllers
{
    [Authorize(Roles = "Utilisateur")]
    public class CamionController : Controller
    {
        private readonly HelmoBiliteRcLmContext _context;

        public CamionController(HelmoBiliteRcLmContext context)
        {
            _context = context;
        }

        // GET: Camion
        public async Task<IActionResult> Index()
        {
              return _context.Camions != null ? 
                          View(await _context.Camions.ToListAsync()) :
                          Problem("Entity set 'HelmoBilite_RC_LMContext.Camions'  is null.");
        }

        // GET: Camion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Camions == null)
            {
                return NotFound();
            }

            var camion = await _context.Camions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camion == null)
            {
                return NotFound();
            }

            return View(camion);
        }

        // GET: Camion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Camion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Marque,Modele,PlaqueImmatriculation,Type,TonnageMaximum,PhotoFile")] Camion camion)
        {
            ModelState.Remove("Photo");
            ModelState.Remove("OrdreLivraisons");
            ModelState.Remove("Statut");
            if (ModelState.IsValid)
            {
                if (camion.PhotoFile != null && camion.PhotoFile.Length > 0)
                {
                    
                    var fileName = Path.GetFileName(camion.PhotoFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "camions", fileName);
                    //Vérifier si le format de l'image est correct
                    var fileExt = Path.GetExtension(fileName).ToLower();
                    if (fileExt != ".jpg" && fileExt != ".png" && fileExt != ".jpeg")
                    {
                        ModelState.AddModelError("PhotoFile", "Le format de l'image n'est pas correct");
                        return View(camion);
                    }
                    await using (var fileSteam = new FileStream(filePath, FileMode.Create))
                    {
                        await camion.PhotoFile.CopyToAsync(fileSteam);
                    }
                    camion.Photo = fileName;
                }
                camion.Statut = "Disponible";
                _context.Add(camion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(camion);
        }

        // GET: Camion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Camions == null)
            {
                return NotFound();
            }

            var camion = await _context.Camions.FindAsync(id);
            if (camion == null)
            {
                return NotFound();
            }
            return View(camion);
        }

        // POST: Camion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Marque,Modele,PlaqueImmatriculation,Type,TonnageMaximum,Photo,Statut")] Camion camion)
        {
            if (id != camion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamionExists(camion.Id))
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
            return View(camion);
        }

        // GET: Camion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Camions == null)
            {
                return NotFound();
            }

            var camion = await _context.Camions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camion == null)
            {
                return NotFound();
            }

            return View(camion);
        }

        // POST: Camion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Camions == null)
            {
                return Problem("Entity set 'HelmoBilite_RC_LMContext.Camions'  is null.");
            }
            var camion = await _context.Camions.FindAsync(id);
            if (camion != null)
            {
                _context.Camions.Remove(camion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CamionExists(int id)
        {
          return (_context.Camions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
