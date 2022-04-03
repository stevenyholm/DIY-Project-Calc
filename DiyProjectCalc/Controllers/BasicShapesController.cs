using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.SharedKernel.Interfaces;
using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Controllers
{
    [Route("/Projects/{projectId:int}/[controller]/[action]")]
    public class BasicShapesController : Controller
    {
        private readonly API.BasicShapesController _api;

        public BasicShapesController(IMapper mapper,
            IRepository<Project> projectRepository)
        {
            this._api = new API.BasicShapesController(mapper, projectRepository);
        }

        // GET: ~/projects/5/basicshapes
        public async Task<IActionResult> Index([FromRoute(Name = "ProjectId")] int projectId)
        {
            var projectDTO = await _api.GetAllForProject(projectId);
            ViewData["ProjectId"] = projectDTO.Id;
            ViewData["ProjectName"] = projectDTO.Name;
            return View(projectDTO.BasicShapes);
        }

        // GET: ~/projects/5/basicshapes/details/5
        public async Task<IActionResult> Details([FromRoute(Name = "ProjectId")] int projectId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicShape = await _api.Get(projectId, id ??default(int));
            if (basicShape == null)
            {
                return NotFound();
            }

            return View(basicShape);
        }

        // GET: ~/projects/5/basicshapes/create
        public IActionResult Create([FromRoute(Name = "ProjectId")] int projectId)
        {
            ViewData["ProjectId"] = projectId;
            return View();
        }

        // POST: ~/projects/5/basicshapes/create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [FromRoute(Name = "ProjectId")] int projectId, 
            [Bind("ShapeType,Number1,Number2,Name,ProjectId")] BasicShapeDTO newBasicShapeDTO
            )
        {
            if (ModelState.IsValid)
            {
                var response = await _api.Post(projectId, newBasicShapeDTO);
                if (response is CreatedAtActionResult)
                {
                    return RedirectToAction(nameof(Index), new { ProjectId = projectId });
                }
            }
            ViewData["ProjectId"] = projectId;
            return View(newBasicShapeDTO);
        }

        // GET: ~/projects/5/basicshapes/edit/5
        public async Task<IActionResult> Edit([FromRoute(Name = "ProjectId")] int projectId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicShapeToUpdate = await _api.Get(projectId, id ?? default(int));
            if (basicShapeToUpdate == null)
            {
                return NotFound();
            }
            return View(basicShapeToUpdate);
        }

        // POST: ~/projects/5/basicshapes/edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [FromRoute(Name = "ProjectId")] int projectId, 
            int id, 
            [Bind("Id,ShapeType,Number1,Number2,Name,ProjectId")] BasicShapeDTO updatedBasicShapeDTO
            )
        {
            if (id != updatedBasicShapeDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _api.Put(projectId, id, updatedBasicShapeDTO);
                if (result is OkResult)
                    return RedirectToAction(nameof(Index), new { ProjectId = projectId });
                else if (result is NotFoundResult)
                    return NotFound();
                else
                    throw new Exception("Error in saving edits to basic shape.");
            }
            return View(updatedBasicShapeDTO);
        }

        // GET: ~/projects/5/basicshapes/delete/5
        public async Task<IActionResult> Delete([FromRoute(Name = "ProjectId")] int projectId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicShape = await _api.Get(projectId, id ?? default(int));
            if (basicShape == null)
            {
                return NotFound();
            }

            return View(basicShape);
        }

        // POST: ~/projects/5/basicshapes/delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromRoute(Name = "ProjectId")] int projectId, int id)
        {
            var response = await _api.Delete(projectId, id);
            return RedirectToAction(nameof(Index), new { ProjectId = projectId });
        }
    }
}
