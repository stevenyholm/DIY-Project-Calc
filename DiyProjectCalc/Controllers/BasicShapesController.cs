using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiyProjectCalc.Models;
using DiyProjectCalc.Repositories;

namespace DiyProjectCalc.Controllers
{
    public class BasicShapesController : Controller
    {
        private readonly IBasicShapeRepository _repository;
        private readonly IProjectRepository _projectRepository;

        public BasicShapesController(IBasicShapeRepository repository, IProjectRepository projectRepository)
        {
            _repository = repository;
            _projectRepository = projectRepository;
        }

        // GET: BasicShapes
        public async Task<IActionResult> Index([FromQuery(Name = "ProjectId")] int projectId)
        {
            var basicShapes = await _repository.GetBasicShapesForProjectAsync(projectId);
            ViewData["ProjectId"] = projectId;
            var project = await _projectRepository.GetProjectAsync(projectId);
            ViewData["ProjectName"] = (project is not null) ? project.Name : default(string);
            return View(basicShapes);
        }

        // GET: BasicShapes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicShape = await _repository.GetBasicShapeAsync(Convert.ToInt32(id));
            if (basicShape == null)
            {
                return NotFound();
            }

            return View(basicShape);
        }

        // GET: BasicShapes/Create
        public IActionResult Create([FromQuery(Name = "ProjectId")] int projectId)
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
                await _repository.AddAsync(basicShape);
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

            var basicShape = await _repository.GetBasicShapeAsync(Convert.ToInt32(id));
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
                    await _repository.UpdateAsync(basicShape);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _repository.BasicShapeExists(basicShape.BasicShapeId)) 
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

            var basicShape = await _repository.GetBasicShapeAsync(Convert.ToInt32(id));
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
            var basicShape = await _repository.GetBasicShapeAsync(Convert.ToInt32(id));
            var projectId = basicShape?.ProjectId;
            await _repository.DeleteAsync(basicShape!);
            return RedirectToAction(nameof(Index), new { ProjectId = projectId });
        }
    }
}
