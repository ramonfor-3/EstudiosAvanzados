using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EstudiosAvanzados.Models;

namespace EstudiosAvanzados.Controllers
{
    public class EstudiantesController : Controller
    {
        enum Estado
        {
            Activo,
            Inactivo
        }

        private readonly RegistroEstudiantesContext _context;

        public EstudiantesController(RegistroEstudiantesContext context)
        {
            _context = context;
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Estudiantes.Where(
                w => w.Estado == Enum.GetName(typeof(Estado), Estado.Activo)
            ).ToListAsync());
        }

        // GET: Estudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiantes = await _context.Estudiantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiantes == null)
            {
                return NotFound();
            }

            return View(estudiantes);
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            return View();
        }

        private int Age(DateTime birthDate)
        {
            var today = DateTime.Today;

            if (birthDate > today)
                return 0;
            else
                return new DateTime((today - birthDate).Ticks).Year - 1;
        }

        // POST: Estudiantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,FechaNacimiento,Curso,Estado")] Estudiantes estudiantes)
        {
            if (ModelState.IsValid)
            {
                var edad = Age(estudiantes.FechaNacimiento);

                if(edad < 16)
                {
                    this.ModelState.AddModelError("FechaNacimiento", "Estudiante debe tener minimo 16 años");
                    return View();
                }

                estudiantes.Estado = "Activo";
                _context.Add(estudiantes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estudiantes);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiantes = await _context.Estudiantes.FindAsync(id);
            if (estudiantes == null)
            {
                return NotFound();
            }
            return View(estudiantes);
        }

        // POST: Estudiantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,FechaNacimiento,Curso,Estado")] Estudiantes estudiantes)
        {
            if (id != estudiantes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var edad = Age(estudiantes.FechaNacimiento);

                if (edad < 16)
                {
                    this.ModelState.AddModelError("FechaNacimiento", "Estudiante debe tener minimo 16 años");
                    return View();
                }
                try
                {
                    _context.Update(estudiantes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudiantesExists(estudiantes.Id))
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
            return View(estudiantes);
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiantes = await _context.Estudiantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiantes == null)
            {
                return NotFound();
            }

            return View(estudiantes);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiantes = await _context.Estudiantes.FindAsync(id);
            //_context.Estudiantes.Remove(estudiantes);
            if (estudiantes.Estado.Trim() == Enum.GetName(typeof(Estado), Estado.Activo))
                estudiantes.Estado = Enum.GetName(typeof(Estado), Estado.Inactivo);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudiantesExists(int id)
        {
            return _context.Estudiantes.Any(e => e.Id == id);
        }
    }
}
