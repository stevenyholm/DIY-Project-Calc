#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DIY_Project_Calc.Data;
using DIY_Project_Calc.Models;

namespace DIY_Project_Calc.Controllers
{
    public class BasicShapesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BasicShapesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BasicShapes
        public async Task<IActionResult> Index([FromQuery(Name = "ProjectId")] string projectId)
        {
            var applicationDbContext = _context.BasicShapes.Include(b => b.Project)
                .Where(b => b.ProjectId == int.Parse(projectId));
            ViewData["ProjectId"] = projectId;
            ViewData["ProjectName"] = _context.Projects.Where(p => p.ProjectId == int.Parse(projectId)).FirstOrDefault().Name;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BasicShapes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicShape = await _context.BasicShapes
                .Include(b => b.Project)
                .FirstOrDefaultAsync(m => m.BasicShapeId == id);
            if (basicShape == null)
            {
                return NotFound();
            }

            return View(basicShape);
        }

        // GET: BasicShapes/Create
        public IActionResult Create([FromQuery(Name = "ProjectId")] string projectId)
        {
            ViewData["ProjectId"] = projectId;
            return View();
        }

        // POST: BasicShapes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BasicShapeId,ShapeType,Number1,Number2,Name,ProjectId")] BasicShape basicShape)
        {
            if (ModelState.IsValid)
            {
                _context.Add(basicShape);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { ProjectId = basicShape.ProjectId });
            }
            ViewData["ProjectId"] = basicShape.ProjectId;
            return View(basicShape);
        }

        // GET: BasicShapes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicShape = await _context.BasicShapes.FindAsync(id);
            if (basicShape == null)
            {
                return NotFound();
            }
            return View(basicShape);
        }

        // POST: BasicShapes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BasicShapeId,ShapeType,Number1,Number2,Name,ProjectId")] BasicShape basicShape)
        {
            if (id != basicShape.BasicShapeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basicShape);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasicShapeExists(basicShape.BasicShapeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { ProjectId = basicShape.ProjectId });
            }
            return View(basicShape);
        }

        // GET: BasicShapes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicShape = await _context.BasicShapes
                .Include(b => b.Project)
                .FirstOrDefaultAsync(m => m.BasicShapeId == id);
            if (basicShape == null)
            {
                return NotFound();
            }

            return View(basicShape);
        }

        // POST: BasicShapes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var basicShape = await _context.BasicShapes.FindAsync(id);
            var projectId = basicShape.ProjectId;
            _context.BasicShapes.Remove(basicShape);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { ProjectId = projectId });
        }

        private bool BasicShapeExists(int id)
        {
            return _context.BasicShapes.Any(e => e.BasicShapeId == id);
        }
    }
}
