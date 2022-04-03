using AutoMapper;
using DiyProjectCalc.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using DiyProjectCalc.SharedKernel.Interfaces;
using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Controllers;
public class ProjectsController : Controller
{
    private readonly API.ProjectsController _api;
    public ProjectsController(IMapper mapper, IRepository<Project> projectRepository)
    {
        _api = new API.ProjectsController(mapper, projectRepository);
    }
    //NOTE: I wrote this controller along with a tutorial - and plan on making the style be consistent with the MS scaffolded code 

    // GET: ~/projects
    public async Task<IActionResult> Index()
    {
        var projects = await _api.Get();
        return View(projects);
    }

    // GET: ~/projects/details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var project = await _api.Get(id ?? default(int));
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    // GET: ~/projects/create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ~/projects/create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectDTO newProjectDTO)
    {
        if (ModelState.IsValid)
        {
            await _api.Post(newProjectDTO);
            return RedirectToAction("Index");
        }
        return View(newProjectDTO);
    }

    // GET: ~/projects/edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id.Value == 0) 
            return NotFound();

        var projectFromDb = await _api.Get(id ?? default(int));
        if (projectFromDb == null) 
            return NotFound();

        return View(projectFromDb);
    }

    // GET: ~/projects/edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProjectDTO updatedProjectDTO)
    {
        if (ModelState.IsValid) 
        {
            await _api.Put(updatedProjectDTO.Id, updatedProjectDTO);
            return RedirectToAction("Index");
        }
        return View(updatedProjectDTO);
    }


    // GET: ~/projects/delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id.Value == 0)
            return NotFound();

        var projectToDelete = await _api.Get(id ?? default(int));
        if (projectToDelete == null)
            return NotFound();

        return View(projectToDelete);
    }

    // GET: ~/projects/delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePOST(int? id)
    {
        if (id == null || id.Value == 0)
            return NotFound();

        var response = await _api.Delete(id ?? default(int));

        if (response is OkResult)
            return RedirectToAction("Index");

        return BadRequest();
    }

}
