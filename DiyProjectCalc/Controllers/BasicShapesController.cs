using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.Core.Interfaces;

namespace DiyProjectCalc.Controllers
{
    public class BasicShapesController : Controller
    {
        private readonly IBasicShapeRepository _repository;
        private readonly IProjectRepository _projectRepository;
        private readonly API.BasicShapesController _api;

        public BasicShapesController(IMapper mapper, IBasicShapeRepository repository, IProjectRepository projectRepository)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _api = new API.BasicShapesController(mapper, _repository, _projectRepository);
        }

        // GET: BasicShapes
        public async Task<IActionResult> Index([FromQuery(Name = "ProjectId")] int projectId)
        {
            var model = await _api.GetAllForProject(projectId);
            ViewData["ProjectId"] = model.ProjectId;
            ViewData["ProjectName"] = model.Name;
            return View(model.BasicShapes);
        }

        // GET: BasicShapes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicShape = await _api.Get(Convert.ToInt32(id));
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
        public async Task<IActionResult> Create([Bind("BasicShapeId,ShapeType,Number1,Number2,Name,ProjectId")] BasicShapeDTO basicShape)
        {
            if (ModelState.IsValid)
            {
                var response = await _api.Post(basicShape);
                if (response is CreatedAtActionResult)
                {
                    return RedirectToAction(nameof(Index), new { ProjectId = basicShape.ProjectId });
                }
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

            var basicShape = await _api.Get(Convert.ToInt32(id));
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
        public async Task<IActionResult> Edit(int id, [Bind("BasicShapeId,ShapeType,Number1,Number2,Name,ProjectId")] BasicShapeDTO basicShape)
        {
            if (id != basicShape.BasicShapeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _api.Put(id, basicShape);
                if (result is OkResult)
                    return RedirectToAction(nameof(Index), new { ProjectId = basicShape.ProjectId });
                else if (result is NotFoundResult)
                    return NotFound();
                else
                    throw new Exception("Error in saving edits to basic shape.");
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

            var basicShape = await _api.Get(Convert.ToInt32(id));
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
            var response = await _api.Delete(id);
            return RedirectToAction(nameof(Index), new { ProjectId = response });
        }
    }
}
