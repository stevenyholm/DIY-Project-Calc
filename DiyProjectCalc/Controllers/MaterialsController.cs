using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.ViewModels;
using DiyProjectCalc.Repositories;

namespace DiyProjectCalc.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly IMaterialRepository _repository;
        private readonly IProjectRepository _projectRepository;
        private readonly IBasicShapeRepository _basicShapeRepository;

        public MaterialsController(IMaterialRepository repository, IProjectRepository projectRepository, IBasicShapeRepository basicShapeRepository)
        {
            this._repository = repository;
            this._projectRepository = projectRepository;
            this._basicShapeRepository = basicShapeRepository;
        }

        // GET: Materials
        public async Task<IActionResult> Index([FromQuery(Name = "ProjectId")] int projectId)
        {
            var materials = await _repository.GetMaterialsForProjectAsync(projectId);
            ViewData["ProjectId"] = projectId;
            var project = await _projectRepository.GetProjectAsync(projectId);
            ViewData["ProjectName"] = (project is not null) ? project.Name : default(string);
            return View(materials);
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _repository.GetMaterialAsync(Convert.ToInt32(id));
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
                await SetBasicShapesForMaterial(model.Material!, selectedBasicShapeIds);
                await _repository.AddAsync(model.Material!);
                return RedirectToAction(nameof(Index), new { ProjectId = model.ProjectId });
            }
            await SetBasicShapesForMaterial(model.Material!, selectedBasicShapeIds);
            model.BasicShapesForProject = await AllBasicShapesForProject(model.ProjectId);
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
            if (id != model.Material?.MaterialId) 
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdateAsync(model.Material, selectedBasicShapeIds);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await MaterialExists(model.Material.MaterialId))
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

            await SetBasicShapesForMaterial(model.Material, selectedBasicShapeIds);
            model.BasicShapesForProject = await AllBasicShapesForProject(model.ProjectId);
            return View(model);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _repository.GetMaterialAsync(Convert.ToInt32(id));
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
            var material = await _repository.GetMaterialAsync(id);
            var projectId = material?.ProjectId;
            await _repository.DeleteAsync(material!);
            return RedirectToAction(nameof(Index), new { ProjectId = projectId });
        }

        //================== Helper functions ========================================================
        private async Task<bool> MaterialExists(int id)
        {
            return await _repository.MaterialExists(id);
        }

        private async Task<MaterialEditViewModel> GetModelAsync(int id) => await GetModelAsync(id, false);
        private async Task<MaterialEditViewModel> GetModelAsync(int id, bool includeMaterial)
        {
            var model = new MaterialEditViewModel();
            if (includeMaterial)
            {
                model.Material = await _repository.GetMaterialAsync(id);
                if (model.Material == null)
                    return null!;
                model.ProjectId = model.Material.ProjectId;
            }
            else
            {
                model.ProjectId = id;
            }
            model.BasicShapesForProject = await AllBasicShapesForProject(model.ProjectId);
            return model;
        }

        private async Task<ICollection<BasicShape>> AllBasicShapesForProject(int projectId) =>
            (ICollection<BasicShape>)await _basicShapeRepository.GetBasicShapesForProjectAsync(projectId);

        private async Task SetBasicShapesForMaterial(Material model, int[] selectedBasicShapeIds)
        {
            var allBasicShapesForProject = await AllBasicShapesForProject(model.ProjectId);

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
