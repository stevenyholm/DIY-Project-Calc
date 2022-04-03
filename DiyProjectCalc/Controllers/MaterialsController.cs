using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.ViewModels;
using AutoMapper;
using DiyProjectCalc.SharedKernel.Interfaces;
using DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;

namespace DiyProjectCalc.Controllers
{
    [Route("/Projects/{projectId:int}/[controller]/[action]")]
    public class MaterialsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Project> _projectRepository;

        public MaterialsController(IMapper mapper,
            IRepository<Project> projectRepository)
        {
            this._mapper = mapper;
            this._projectRepository = projectRepository;

        }

        // GET: ~/projects/5/materials
        public async Task<IActionResult> Index([FromRoute(Name = "ProjectId")] int projectId)
        {
            var project = await LoadProjectDataWithMaterials(projectId);
            ViewData["ProjectId"] = projectId;
            ViewData["ProjectName"] = (project is not null) ? project.Name : default(string);
            return View(project?.Materials);
        }

        // GET: ~/projects/5/materials/details/5
        public async Task<IActionResult> Details([FromRoute(Name = "ProjectId")] int projectId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await LoadProjectDataWithMaterials(projectId);
            var material = project?.GetMaterial(id ?? default(int));
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: ~/projects/5/materials/create
        public async Task<IActionResult> Create([FromRoute(Name = "ProjectId")] int projectId)
        {
            var project = await LoadAllProjectData(projectId);

            return View(MapToViewModel(project));
        }

        // POST: ~/projects/5/materials/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [FromRoute(Name = "ProjectId")] int projectId, 
            MaterialEditViewModel viewModel, 
            int[] selectedBasicShapeIds
            )
        {
            var project = await LoadAllProjectData(projectId);
            SetBasicShapesForMaterial(viewModel.Material!, selectedBasicShapeIds, project);

            if (ModelState.IsValid) 
            {
                project.AddMaterial(viewModel.Material!);
                await _projectRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { ProjectId = projectId });
            }
            viewModel.BasicShapesForProject = project.BasicShapes;
            return View(viewModel);
        }

        // GET: ~/projects/5/materials/edit/5
        public async Task<IActionResult> Edit([FromRoute(Name = "ProjectId")] int projectId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await LoadProjectDataWithMaterials(projectId);
            var viewModel = MapToViewModel(project, id ?? default(int));
            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: ~/projects/5/materials/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [FromRoute(Name = "ProjectId")] int projectId, 
            int id, 
            MaterialEditViewModel viewModel, 
            int[] selectedBasicShapeIds
            )
        {
            if (id != viewModel.Material?.Id) 
            {
                return NotFound();
            }

            var project = await LoadAllProjectData(projectId);
            SetBasicShapesForMaterial(viewModel.Material!, selectedBasicShapeIds, project);

            if (ModelState.IsValid)
            {
                try
                {
                    project.UpdateMaterial(viewModel.Material!, _mapper);
                    await _projectRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! project.MaterialExists(viewModel.Material.Id))
                    {
                        return NotFound(); 
                    }
                    else
                    {
                        throw;  
                    }
                }
                return RedirectToAction(nameof(Index), new { ProjectId = projectId });
            }

            viewModel.BasicShapesForProject = project.BasicShapes;

            return View(viewModel);
        }

        // GET: ~/projects/5/materials/delete/5
        public async Task<IActionResult> Delete([FromRoute(Name = "ProjectId")] int projectId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await LoadProjectDataWithMaterials(projectId);
            var material = project?.GetMaterial(id ?? default(int));
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // POST: ~/projects/5/materials/delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromRoute(Name = "ProjectId")] int projectId, int id)
        {
            var project = await LoadProjectDataWithMaterials(projectId);
            project?.RemoveMaterial(id);
            await _projectRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { ProjectId = projectId });
        }

        //================== Helper functions ========================================================


        private async Task<Project> LoadProjectDataWithMaterials(int projectId)
        {
            var projectSpec = new ProjectWithMaterialsSpec(projectId);
            var project = await _projectRepository.GetBySpecAsync(projectSpec);
            return project!;
        }

        private async Task<Project> LoadAllProjectData(int projectId)
        {
            var projectSpec = new ProjectWithMaterialsSpec(projectId);
            var project = await _projectRepository.GetBySpecAsync(projectSpec);
            return project!;
        }

        private MaterialEditViewModel MapToViewModel(Project project, int materialId = -1)
        {
            var model = new MaterialEditViewModel();
            if (materialId > 0)
            {
                model.Material = project.Materials.FirstOrDefault(m => m.Id == materialId);
            }
            model.ProjectId = project.Id;
            model.BasicShapesForProject = project.BasicShapes;
            return model;
        }

        private void SetBasicShapesForMaterial(Material material, int[] selectedBasicShapeIds, Project project)
        {
            var basicShapesToAdd = project.BasicShapes
                .Where(basicShape => selectedBasicShapeIds.Any(selectedId => selectedId == basicShape.Id))
                .Where(basicShapeFromProject => !material.BasicShapes.Any(basicShapeFromMaterial => basicShapeFromMaterial.Id == basicShapeFromProject.Id));
            foreach (var basicShape in basicShapesToAdd)
            {
                material.AddBasicShape(basicShape);
            }

            bool isEditView = (material.Id != default(int));
            if (isEditView)
            {
                var basicShapesToRemove = material.BasicShapes
                    .Where(b => !selectedBasicShapeIds.Any(s => s == b.Id)).ToList();
                foreach (var basicShape in basicShapesToRemove)
                {
                    material.RemoveBasicShape(basicShape);
                }
            }
        }

    }
}
