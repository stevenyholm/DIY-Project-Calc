#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiyProjectCalc.Data;
using DiyProjectCalc.Models;
using DiyProjectCalc.ViewModels;

namespace DiyProjectCalc.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Materials
        public async Task<IActionResult> Index([FromQuery(Name = "ProjectId")] int projectId)
        {
            var applicationDbContext = _context.Materials.Include(m => m.Project).Include(m => m.BasicShapes)
                .Where(b => b.ProjectId == projectId);
            ViewData["ProjectId"] = projectId;
            var project = _context.Projects.Where(p => p.ProjectId == projectId).FirstOrDefault();
            ViewData["ProjectName"] = (project is not null) ? project.Name : default(string);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .Include(m => m.Project)
                .Include(m => m.BasicShapes)
                .FirstOrDefaultAsync(m => m.MaterialId == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: Materials/Create
        public async Task<IActionResult> Create([FromQuery(Name = "ProjectId")] int projectId)
        {
            return View(await GetModelAsync(projectId));
        }

        // POST: Materials/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialEditViewModel model, int[] selectedBasicShapeIds)
        {
            if (ModelState.IsValid)
            {
                SetBasicShapesForMaterial(model.Material, selectedBasicShapeIds);
                _context.Add(model.Material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { ProjectId = model.ProjectId });
            }
            SetBasicShapesForMaterial(model.Material, selectedBasicShapeIds);
            model.BasicShapesForProject = AllBasicShapesForProject(model.ProjectId);
            return View(model);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await GetModelAsync(id ?? default(int), true);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Materials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialEditViewModel model, int[] selectedBasicShapeIds)
        {
            if (id != model.Material.MaterialId) 
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var materialToSave = _context.Materials
                        .Include(m => m.BasicShapes)
                        .FirstOrDefault(m => m.MaterialId == model.Material.MaterialId);

                    if (materialToSave is not null) //TODO: change "== null" to "is null" across the project
                    {
                        _context.Entry(materialToSave).CurrentValues.SetValues(model.Material);
                        SetBasicShapesForMaterial(materialToSave, selectedBasicShapeIds);
                        _context.Update(materialToSave);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new DbUpdateConcurrencyException();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    //TODO: handle errors more gracefully than returning page NotFound or throwing an error
                    if (!MaterialExists(model.Material.MaterialId))
                    {
                        return NotFound(); 
                    }
                    else
                    {
                        throw;  
                    }
                }
                return RedirectToAction(nameof(Index), new { ProjectId = model.ProjectId });
            }

            SetBasicShapesForMaterial(model.Material, selectedBasicShapeIds);
            model.BasicShapesForProject = AllBasicShapesForProject(model.ProjectId);
            return View(model);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.MaterialId == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            var projectId = material.ProjectId;
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { ProjectId = projectId });
        }

        //================== Helper functions ========================================================
        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.MaterialId == id);
        }

        private async Task<MaterialEditViewModel> GetModelAsync(int id) => await GetModelAsync(id, false);
        private async Task<MaterialEditViewModel> GetModelAsync(int id, bool includeMaterial)
        {
            var model = new MaterialEditViewModel();
            if (includeMaterial)
            {
                model.Material = await _context.Materials
                    .Include(m => m.BasicShapes)
                    .AsNoTracking() 
                    .FirstOrDefaultAsync(m => m.MaterialId == id);
                if (model.Material == null)
                    return null;
                model.ProjectId = model.Material.ProjectId;
            }
            else
            {
                model.ProjectId = id;
            }
            model.BasicShapesForProject = AllBasicShapesForProject(model.ProjectId);
            return model;
        }

        private List<BasicShape> AllBasicShapesForProject(int projectId) =>
            _context.BasicShapes.Where(b => b.ProjectId == projectId).ToList();

        private void SetBasicShapesForMaterial(Material model, int[] selectedBasicShapeIds)
        {
            var allBasicShapesForProject = AllBasicShapesForProject(model.ProjectId);

            var basicShapesToAdd = allBasicShapesForProject
                .Where(b => selectedBasicShapeIds.Any(s => s == b.BasicShapeId))
                .Where(b => !model.BasicShapes.Any(m => m.BasicShapeId == b.BasicShapeId));
            foreach(var basicShape in basicShapesToAdd)
            {
                model.BasicShapes.Add(basicShape);
            }

            bool isEditView = (model.MaterialId != default(int));
            if (isEditView)
            {
                var basicShapesToRemove = model.BasicShapes
                    .Where(b => !selectedBasicShapeIds.Any(s => s == b.BasicShapeId)).ToList();
                foreach (var basicShape in basicShapesToRemove)
                {
                    model.BasicShapes.Remove(basicShape);
                }
            }
        }

    }
}
