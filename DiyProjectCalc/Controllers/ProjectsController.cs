using AutoMapper;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiyProjectCalc.Controllers;
public class ProjectsController : Controller
{
    private readonly IProjectRepository _repository;
    private readonly API.ProjectsController _api;
    public ProjectsController(IMapper mapper, IProjectRepository repository)
    {
        this._repository = repository;
        _api = new API.ProjectsController(mapper, _repository);
    }
    //NOTE: I wrote this controller along with a tutorial - and plan on making the style be consistent with the MS scaffolded code 

    public async Task<IActionResult> Index()
    {
        var projects = await _api.Get();
        return View(projects);
    }

    // GET: Projects/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var project = await _api.Get(Convert.ToInt32(id));
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectDTO obj)
    {
        if (ModelState.IsValid)
        {
            await _api.Post(obj);
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id.Value == 0) 
            return NotFound();

        var projectFromDb = await _api.Get(Convert.ToInt32(id));
        if (projectFromDb == null) 
            return NotFound();

        return View(projectFromDb);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProjectDTO obj)
    {
        if (ModelState.IsValid) 
        {
            await _api.Put(obj.Id, obj);
            return RedirectToAction("Index");
        }
        return View(obj);
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id.Value == 0)
            return NotFound();

        var projectFromDb = await _api.Get(Convert.ToInt32(id));
        if (projectFromDb == null)
            return NotFound();

        return View(projectFromDb);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePOST(int? id)
    {
        if (id == null || id.Value == 0)
            return NotFound();

        var response = await _api.Delete(Convert.ToInt32(id));

        if (response is OkResult)
            return RedirectToAction("Index");

        return BadRequest();
    }

}
